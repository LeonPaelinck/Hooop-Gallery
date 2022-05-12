using Domain;
using Microsoft.EntityFrameworkCore;
using Project3H04.Server.Data;
using Project3H04.Shared.Kunstenaars;
using Project3H04.Shared.Kunstwerken;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Project3H04.Shared.DTO;

namespace Project3H04.Server.Services
{
    public class KunstenaarService : IKunstenaarService
    {
        private readonly ApplicationDbcontext dbContext;

        public KunstenaarService(ApplicationDbcontext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<KunstenaarResponse.Detail> GetDetailAsync(int id)
        {
            //kunstwerk.fotos nog includen !!!

            var x = (Kunstenaar)dbContext.Gebruikers.OfType<Kunstenaar>().Include(k => k.Kunstwerken).ThenInclude(x => x.Fotos).SingleOrDefault(x => x.GebruikerId == id);
            //include van fotos ...
            var k = await Task.Run(() => new Kunstenaar_DTO
            {
                Gebruikersnaam = x.Gebruikersnaam,
                GebruikerId = x.GebruikerId,
                Details = x.Details,
                Email = x.Email,
                //deze nog goe omzette
                Kunstwerken = x.Kunstwerken.Select(x => new Kunstwerk_DTO.Index
                {
                    Id = x.Id,
                    Naam = x.Naam,
                    HoofdFoto = new Foto_DTO(x.Fotos.FirstOrDefault().Naam, x.Fotos.FirstOrDefault().Locatie), //enkel eerste foto is nodig voor index
                    Prijs = x.Prijs
                }).ToList(),
                Fotopad = x.FotoPad
                //,Veilingen = (ICollection<Shared.DTO.Veiling_DTO>)x.Veilingen //omzetten naar dto
            });

            return new KunstenaarResponse.Detail() { Kunstenaar = k };
        }

        public async Task<KunstenaarResponse.Detail> GetKunstenaarByEmail(string email)
        {
            var x = (Kunstenaar)dbContext.Gebruikers.OfType<Kunstenaar>().Include(k => k.Kunstwerken).ThenInclude(x => x.Fotos).Include(x => x.Abonnenment).ThenInclude(x => x.AbonnementType).SingleOrDefault(x => x.Email == email);
            //include van fotos ...
            if (x == null)
            {
                return new KunstenaarResponse.Detail();
            }
            var k = await Task.Run(() => new Kunstenaar_DTO
            {
                Gebruikersnaam = x.Gebruikersnaam,
                GebruikerId = x.GebruikerId,
                GeboorteDatum = x.Geboortedatum,
                Details = x.Details,
                Email = x.Email,
                //deze nog goe omzette
                Kunstwerken = x.Kunstwerken.Select(x => new Kunstwerk_DTO.Index
                {
                    Id = x.Id,
                    Naam = x.Naam,
                    HoofdFoto = new Foto_DTO(x.Fotos.FirstOrDefault().Naam, x.Fotos.FirstOrDefault().Locatie), //enkel eerste foto is nodig voor index
                    Prijs = x.Prijs
                }).ToList(),
                Abonnement = new Abonnement_DTO
                {
                    Id = x.AbonnenmentId,
                    StartDatum = x.Abonnenment.StartDatum,
                    EindDatum = x.Abonnenment.EindDatum,
                    AbonnementType = new AbonnementType_DTO
                    {
                        Naam = x.Abonnenment.AbonnementType.Naam,
                        Verlooptijd = x.Abonnenment.AbonnementType.Verlooptijd,
                        Prijs = x.Abonnenment.AbonnementType.Prijs
                    }
                },
                Fotopad = x.FotoPad
                //,Veilingen = (ICollection<Shared.DTO.Veiling_DTO>)x.Veilingen //omzetten naar dto
            });

            return new KunstenaarResponse.Detail() { Kunstenaar = k };
        }

        public async Task<KunstenaarResponse.Index> GetKunstenaars(string term, int take, bool recentArtists)
        {
            term ??= "";

            var kunstenaars = await dbContext.Gebruikers.OfType<Kunstenaar>()
                .Include(x => x.Kunstwerken)
                .ThenInclude(x => x.Fotos)
                .Select(x => new Kunstenaar_DTO
                {
                    Gebruikersnaam = x.Gebruikersnaam,
                    GebruikerId = x.GebruikerId,
                    DatumCreatie = x.DatumCreatie,
                    Fotopad = x.FotoPad,
                    GeboorteDatum = x.Geboortedatum,
                    GeboortedatumShort = x.Geboortedatum.ToShortDateString(),
                    Details = x.Details,
                    Kunstwerken = x.Kunstwerken.Select(x => new Kunstwerk_DTO.Index
                    {
                        Id = x.Id,
                        Naam = x.Naam,
                        HoofdFoto = new Foto_DTO(x.Fotos.FirstOrDefault().Naam, x.Fotos.FirstOrDefault().Locatie),
                        Prijs = x.Prijs
                    }).ToList(),
                }).Where(k => k.Gebruikersnaam.Contains(term)).Take(take)
                .ToListAsync();

            if (recentArtists)
                kunstenaars = kunstenaars.OrderByDescending(x => x.DatumCreatie).ToList();

            return new KunstenaarResponse.Index() { Kunstenaars = kunstenaars };
        }
    }
}
