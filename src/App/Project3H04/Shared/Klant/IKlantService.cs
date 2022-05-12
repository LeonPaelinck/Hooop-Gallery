using Project3H04.Shared.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project3H04.Shared.Klant {
    public interface IKlantService {
        Task<KlantResponse.Detail> GetKlantById(int id);
        Task<KlantResponse.Detail> GetKlantByEmail(string email);
        Task<KlantResponse.Create> CreateAsync(Klant_DTO klant);
    }
}
