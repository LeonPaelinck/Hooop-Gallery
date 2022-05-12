using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project3H04.Shared.Kunstenaars {
    public interface IKunstenaarService {
        // List<Kunstenaar_DTO> Kunstenaars { get; set; }
        Task<KunstenaarResponse.Detail> GetKunstenaarByEmail(string email);
        Task<KunstenaarResponse.Detail> GetDetailAsync(int id);
        Task<KunstenaarResponse.Index> GetKunstenaars(string term, int take, bool recentArtists);
    }
}
