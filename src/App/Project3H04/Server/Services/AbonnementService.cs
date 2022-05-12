using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain;
using Microsoft.EntityFrameworkCore;
using Project3H04.Server.Data;
using Project3H04.Shared.Abonnementen;
using Project3H04.Shared.DTO;

namespace Project3H04.Server.Services {
    public class AbonnementService : IAbonnementService {
        private readonly ApplicationDbcontext DbContext;

        public AbonnementService(ApplicationDbcontext dbContext) {
            DbContext = dbContext;
        }

        public async Task<IList<Abonnement_DTO>> GetAllAbonnementen() {
            var abonnementen = await DbContext.Abonnementen.Select(x => new Abonnement_DTO {
                Id = x.Id,
                StartDatum = x.StartDatum,
                EindDatum = x.EindDatum,
                AbonnementType = new AbonnementType_DTO {
                    Naam = x.AbonnementType.Naam,
                    Verlooptijd = x.AbonnementType.Verlooptijd,
                    Prijs = x.AbonnementType.Prijs
                }
            }).ToListAsync();

            return abonnementen;
        }

        public async Task<IList<AbonnementType_DTO>> GetAllAbonnementTypes() {
            var abonnementTypes = await DbContext.AbonnementTypes.Select(x => new AbonnementType_DTO {
                Naam = x.Naam,
                Verlooptijd = x.Verlooptijd,
                Prijs = x.Prijs
            }).ToListAsync();

            return abonnementTypes;
        }

        public async Task<Abonnement_DTO> UpdateSubscription(Abonnement_DTO abonnement) {
            var kunstenaar = await DbContext.Gebruikers.OfType<Kunstenaar>().Include(k => k.Abonnenment).ThenInclude(a => a.AbonnementType).SingleOrDefaultAsync(k => k.AbonnenmentId.Equals(abonnement.Id));
            if ((kunstenaar == null) || (kunstenaar.Abonnenment.AbonnementType.Naam.Equals(abonnement.AbonnementType.Naam))){
                return null;
            }

            var abonnementType = await DbContext.AbonnementTypes.SingleOrDefaultAsync(a => a.Naam.Equals(abonnement.AbonnementType.Naam));
            kunstenaar.Abonnenment.AbonnementType = abonnementType;
            kunstenaar.Abonnenment.SetBothData(abonnement.StartDatum);

            DbContext.Gebruikers.Update(kunstenaar);
            await DbContext.SaveChangesAsync();

            return abonnement;
        }
    }
}
