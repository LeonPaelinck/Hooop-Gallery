using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain;
using Microsoft.EntityFrameworkCore;
using Project3H04.Server.Data;
using Project3H04.Shared.DTO;
using Project3H04.Shared.Kunstenaars;
using Project3H04.Shared.Kunstwerken;
using Project3H04.Shared.Veilingen;

namespace Project3H04.Server.Services {
    public class VeilingService : IVeilingService {
        private readonly ApplicationDbcontext _dbContext;

        public VeilingService(ApplicationDbcontext dbContext) {
            _dbContext = dbContext;
        }

        public async Task<Veiling_DTO> GetVeilingByKunstwerkId(int id) {
            var veiling = _dbContext.Veilingen.SingleOrDefault(v => v.Kunstwerk.Id == id);
            if (veiling != null)
                return await GetVeilingById(veiling.Id);
            return null; //TODO: FOUT IMPLEMENTEREN
        }

        public async Task<Veiling_DTO> GetVeilingById(int id) {
            var x = await _dbContext.Veilingen
                .Include(v => v.Kunstwerk).ThenInclude(k => k.Fotos)
                .Include(v => v.BodenOpVeiling).ThenInclude(b => b.Klant)
                .FirstOrDefaultAsync(v => v.Id == id);

            var v = await Task.Run(() => new Veiling_DTO {
                Id = x.Id,
                StartDatum = x.StartDatum,
                EindDatum = x.EindDatum,
                MinPrijs = x.MinPrijs,
                Kunstwerk = new Kunstwerk_DTO.Detail {
                    Id = x.Kunstwerk.Id,
                    Naam = x.Kunstwerk.Naam,
                    Prijs = x.Kunstwerk.Prijs,
                    Fotos = x.Kunstwerk.Fotos.Select(x => new Foto_DTO {
                        Id = x.Id,
                        Naam = x.Naam,
                       Locatie = x.Locatie,
                       Uploaded = true
                    }).ToList(),
                },
                BodenOpVeiling = x.BodenOpVeiling.ToList().Select(x => new Bod_DTO {
                    BodPrijs = x.BodPrijs,
                    Datum = x.Datum,
                    Klant = new Klant_DTO {
                        GebruikerId = x.Klant.GebruikerId,
                        Gebruikersnaam = x.Klant.Gebruikersnaam,
                        GeboorteDatum = x.Klant.Geboortedatum,
                        Email = x.Klant.Email,
                    }
                }).OrderByDescending(b => b.BodPrijs)
            });

            return v;
        }

        public async Task<bool> AddBodToVeiling(Bod_DTO bod, int veilingId) {
             var veiling = _dbContext.Veilingen
                 .Include(v => v.Kunstwerk).ThenInclude(k => k.Fotos)
                 .Include(v => v.BodenOpVeiling).ThenInclude(b => b.Klant)
                 .SingleOrDefault(v => v.Id == veilingId);

             var klant = (Klant)_dbContext.Gebruikers.SingleOrDefault(g => g.GebruikerId == bod.Klant.GebruikerId);

             if (veiling == null)
                 return false;
             if (veiling.BodenOpVeiling.Any() && veiling.HoogsteBod.KlantId == bod.Klant.GebruikerId)
                 return false; //Mag zichzelf niet overbieden

             string oudeHighestBidder = null;
             if (veiling.BodenOpVeiling.Any())
                oudeHighestBidder = veiling.HoogsteBod.Klant.Email;

             veiling.VoegBodToe(klant, bod.BodPrijs, bod.Datum);

            _dbContext.Veilingen.Update(veiling);
            await _dbContext.SaveChangesAsync();

            if (oudeHighestBidder == null)
                return true;

            //Mail sturen naar de vorige bieder dat hij outboden is
            var response = MailService.SendMail(oudeHighestBidder, 
                $"There is a new bid that is higher than yours on '<b>{veiling.Kunstwerk.Naam}</b>'<br/>" + 
                $"The bidding will last until {veiling.EindDatum}",
                "Update: You have been outbid.");
            if (!response.IsSuccessful)
                Console.WriteLine(response.Content);

            return true;
        }

        public async Task<bool> AddBodToKunstwerk(Bod_DTO bod, int kunstwerkId) {
            var veiling = _dbContext.Veilingen.SingleOrDefault(v => v.Kunstwerk.Id == kunstwerkId);
            if (veiling == null)
                return false;

            return await AddBodToVeiling(bod, veiling.Id);
        }

        public async Task<bool> CreateVeiling(Veiling_DTO veiling) {
            var kunstwerk = await _dbContext.Kunstwerken
                .Include(x => x.Fotos)
                .Include(x => x.Kunstenaar)
                .ThenInclude(x => x.Abonnenment)
                .ThenInclude(x => x.AbonnementType)
                .SingleOrDefaultAsync(x => x.Id.Equals(veiling.Kunstwerk.Id));

            kunstwerk.IsVeilbaar = true;
            kunstwerk.TeKoop = false;

            var v = new Veiling(veiling.StartDatum, veiling.EindDatum, veiling.MinPrijs, kunstwerk);

            await _dbContext.Veilingen.AddAsync(v);
            await _dbContext.SaveChangesAsync();

            return true;
        }

        public async Task<bool> EditVeiling(Veiling_DTO veiling) {
            var v = await _dbContext.Veilingen.SingleOrDefaultAsync(x => x.Id.Equals(veiling.Id));

            v.EindDatum = veiling.EindDatum;
            v.StartDatum = veiling.StartDatum;
            v.MinPrijs = veiling.MinPrijs;

            _dbContext.Veilingen.Update(v);

            return true;
        }

        public async Task<List<Veiling_DTO>> GetVeilingen(string term, int take, bool almostFinishedVeilingen) {
            var veilings = await _dbContext.Veilingen.Select(x => new Veiling_DTO {
                Id = x.Id,
                StartDatum = x.StartDatum,
                EindDatum = x.EindDatum,
                MinPrijs = x.MinPrijs,
                Kunstwerk = new Kunstwerk_DTO.Detail {
                    Id = x.Kunstwerk.Id,
                    Naam = x.Kunstwerk.Naam,
                    Prijs = x.Kunstwerk.Prijs,
                    Kunstenaar = new Kunstenaar_DTO
                    {
                        Gebruikersnaam = x.Kunstwerk.Kunstenaar.Gebruikersnaam
                    },
                    Fotos = x.Kunstwerk.Fotos.Select(x => new Foto_DTO {
                        Id = x.Id,
                        Naam = x.Naam
                    }).ToList(),
                },                
                BodenOpVeiling = x.BodenOpVeiling.Select(x => new Bod_DTO {
                    BodPrijs = x.BodPrijs,
                    Datum = x.Datum,
                    Klant = new Klant_DTO {
                        GebruikerId = x.Klant.GebruikerId,
                        Gebruikersnaam = x.Klant.Gebruikersnaam,
                        GeboorteDatum = x.Klant.Geboortedatum,
                        Email = x.Klant.Email,
                    }
                }).OrderByDescending(b => b.BodPrijs).ToList()
            }).Where(k => k.Kunstwerk.Naam.Contains(term)).Take(take).ToListAsync();

            return almostFinishedVeilingen ? veilings.OrderByDescending(x => x.EindDatum).ToList() : veilings.OrderByDescending(x => x.StartDatum).ToList();
        }
    }
}
