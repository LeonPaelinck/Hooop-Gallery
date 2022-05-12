using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Project3H04.Shared.DTO;

namespace Project3H04.Shared.Abonnementen {
    public interface IAbonnementService {
        public Task<IList<Abonnement_DTO>> GetAllAbonnementen();
        public Task<IList<AbonnementType_DTO>> GetAllAbonnementTypes();
        public Task<Abonnement_DTO> UpdateSubscription(Abonnement_DTO abonnement);
    }
}
