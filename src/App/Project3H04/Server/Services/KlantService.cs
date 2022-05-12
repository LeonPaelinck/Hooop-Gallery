using Domain;
using Microsoft.EntityFrameworkCore;
using Project3H04.Server.Data;
using Project3H04.Shared.DTO;
using Project3H04.Shared.Klant;
using Project3H04.Shared.Kunstwerken;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Project3H04.Server.Services {
    public class KlantService : IKlantService {
        private readonly ApplicationDbcontext DbContext;

        public KlantService(ApplicationDbcontext dbContext) {
            DbContext = dbContext;
        }

        public async Task<KlantResponse.Detail> GetKlantByEmail(string email) {
            var x = DbContext.Gebruikers.OfType<Klant>().FirstOrDefault(k => k.Email == email);
            if(x == null)
            {
                return new KlantResponse.Detail();
            }
            var k = await Task.Run(() => new Klant_DTO {
                Gebruikersnaam = x.Gebruikersnaam,
                GebruikerId = x.GebruikerId,
                GeboorteDatum = x.Geboortedatum,
                Email = x.Email,
                Fotopad = x.FotoPad,
                Details = x.Details
            });

            return new KlantResponse.Detail() { Klant = k };
        }

        public async Task<KlantResponse.Detail> GetKlantById(int id) {
            var x = (Klant)DbContext.Gebruikers.OfType<Klant>()/*NTH.Include(k => k.Boden).Include(k => k.Bestellingen).ThenInclude(x => x.WinkelmandKunstwerken)*/.SingleOrDefault(x => x.GebruikerId == id);
            //include van fotos ...
            Klant_DTO k = await Task.Run(() => new Klant_DTO {
                Gebruikersnaam = x.Gebruikersnaam,
                GebruikerId = x.GebruikerId,
                GeboorteDatum = x.Geboortedatum,
                Email = x.Email,
                Fotopad = x.FotoPad,
                Details = x.Details
            });

            return new KlantResponse.Detail() { Klant = k };
        }

        public async Task<KlantResponse.Create> CreateAsync(Klant_DTO klant) {
            //email uniek
            KlantResponse.Create response = new();
            if (!DbContext.Gebruikers.Any(x => x.Email == klant.Email)) {
                var k = new Klant(klant.Gebruikersnaam, klant.GeboorteDatum, klant.Email, klant.Fotopad, klant.Details);
                DbContext.Gebruikers.Add(k);
                await DbContext.SaveChangesAsync();
                response.Message = "succes";
            } else {
                response.Message = "fail";
            }
                
            return response;
        }
    }
}
