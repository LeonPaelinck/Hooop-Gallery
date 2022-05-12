using Domain;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Mollie.Api.Client;
using Mollie.Api.Client.Abstract;
using Mollie.Api.Models.Payment.Response;
using Project3H04.Server.Data;
using Project3H04.Shared.DTO;
using Project3H04.Shared.Kunstwerken;
using Project3H04.Shared.Order;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Project3H04.Server.Services {
    public class OrderService : IOrderService {
        private readonly ApplicationDbcontext DbContext;

        public OrderService(ApplicationDbcontext dbContext) {
            DbContext = dbContext;
        }

        public IList<Kunstwerk_DTO.Detail> CartKunstwerken { get; set; } = new List<Kunstwerk_DTO.Detail>();

        public IList<Kunstwerk_DTO.Detail> GetCartKunstwerken() {
            return CartKunstwerken.ToList();
        }

        public void AddKunstwerk(Kunstwerk_DTO.Detail kunstwerk) {
            CartKunstwerken.Add(kunstwerk);
        }

        public void RemoveKunstwerk(Kunstwerk_DTO.Detail kunstwerk) {
            CartKunstwerken.Remove(kunstwerk);
        }

        public async Task<int> PostOrderAsync(Bestelling_DTO.Create bestelling)  {
            //besteling persisteren in db (dit gebeurt al voordat de betaling wordt uitgevoerd)
            //Transaction = await DbContext.Database.BeginTransactionAsync();
            //DbContext.Database.UseTransaction(Transaction.GetDbTransaction());

            var kunstwerkids = bestelling.WinkelmandKunstwerken.Select(k => k.Id);
            var kunstwerken = DbContext.Kunstwerken.Where(k => kunstwerkids.Contains(k.Id)).ToList();
            foreach (var item in kunstwerken) {
                item.TeKoop = false;             //alle kunstwerken in de bestelling krijgen te koop = false
            }
            var adres = new Address(bestelling.Land, bestelling.Gemeente, bestelling.Postcode, bestelling.Straat);
            var b = new Bestelling(DateTime.UtcNow, adres/* bestelling.Straat, bestelling.Postcode, bestelling.Gemeente,*/, bestelling.PaymentId, bestelling.TotalePrijs, kunstwerken);
            var k = DbContext.Gebruikers.OfType<Klant>().FirstOrDefault(k => k.GebruikerId == 3);
            k.Bestellingen.Add(b);

            DbContext.Kunstwerken.UpdateRange(kunstwerken);
            await DbContext.Bestellingen.AddAsync(b);
            await DbContext.SaveChangesAsync();

            return b.Id;
        }

        public async Task RemoveBestelling(string id)  {
            //als een bestelling geanulleerd wordt, halen we deze uit de db
            // DbContext.Database.UseTransaction(Transaction.GetDbTransaction());
            /*            if (Transaction != null)
                            await Transaction.RollbackAsync();*/

            var b = DbContext.Bestellingen.Include(k => k.WinkelmandKunstwerken).FirstOrDefault(b => b.PaymentId == id);
            if (b == null) return;

            foreach (var item in b.WinkelmandKunstwerken) {
                item.TeKoop = true; //bestelling wordt geannuleerd, kunstwerk kan weer gekocht worden
            }

            DbContext.UpdateRange(b.WinkelmandKunstwerken);
            DbContext.Bestellingen.Remove(b);
            await DbContext.SaveChangesAsync();
        }

        public bool Bestellingexists(int id) {
            var b = DbContext.Bestellingen.FirstOrDefault(b => b.Id == id);

            return b != null;
        }

        public async Task PutOrderAsync(string id, int bestellingId) {
            var b = DbContext.Bestellingen.FirstOrDefault(b => b.Id == bestellingId);
            b.PaymentId = id;

            DbContext.Update(b);
            await DbContext.SaveChangesAsync();
        }

        public async Task<OrderResponse.Detail> GetUserOrders(string email) {
            var gebruiker = await DbContext.Gebruikers.OfType<Klant>().Include(k => k.Bestellingen).ThenInclude(b => b.WinkelmandKunstwerken).ThenInclude(k => k.Fotos).FirstOrDefaultAsync(k => k.Email.Equals(email));

            var bestellingen = gebruiker.Bestellingen.Select(x => new Bestelling_DTO.Index {
                Id = x.Id,
                Datum = x.Datum,
                Gemeente = x.Adres.Gemeente,
                Postcode = x.Adres.Postcode,
                Straat = x.Adres.Straat,
                TotalePrijs = x.TotalePrijs,
                WinkelmandKunstwerken = x.WinkelmandKunstwerken.Select(x => new Kunstwerk_DTO.Detail {
                    Naam = x.Naam,
                    Prijs = x.Prijs,
                    HoofdFoto = x.Fotos.Select(x => new Foto_DTO {
                        Id = x.Id,
                        Naam = x.Naam,
                        Locatie = x.Locatie                        
                    }).FirstOrDefault()
                }).ToList()

            }).ToList();

            return new OrderResponse.Detail() {
                Bestellingen = bestellingen
            };
        }

        public async Task<Bestelling_DTO.Index> GetBestelling(int id) {
            return await DbContext.Bestellingen.Include(x => x.WinkelmandKunstwerken).Select(x => new Bestelling_DTO.Index {
                Id = x.Id,
                WinkelmandKunstwerken = x.WinkelmandKunstwerken.Select(x => new Kunstwerk_DTO.Detail {
                    Id = x.Id,
                    Naam = x.Naam,
                    Fotos = x.Fotos.Select(x => new Foto_DTO {
                        Locatie = x.Locatie,
                        Id = x.Id,
                        Naam = x.Naam
                    }).ToList(),
                    Prijs = x.Prijs,
                    HoofdFoto = x.Fotos.Select(x => new Foto_DTO {
                        Locatie = x.Locatie,
                        Id = x.Id,
                        Naam = x.Naam
                    }).FirstOrDefault()
                }).ToList()
            }).FirstOrDefaultAsync(b => b.Id == id);
        }
    }
}
