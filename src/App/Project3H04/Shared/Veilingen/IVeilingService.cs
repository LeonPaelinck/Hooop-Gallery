using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Project3H04.Shared.DTO;

namespace Project3H04.Shared.Veilingen {
    public interface IVeilingService {
        Task<Veiling_DTO> GetVeilingById(int id);
        Task<Veiling_DTO> GetVeilingByKunstwerkId(int id);
        Task<bool> AddBodToVeiling(Bod_DTO bod, int veilingId);
        Task<bool> AddBodToKunstwerk(Bod_DTO bod, int kunstwerkId);
        Task<bool> CreateVeiling(Veiling_DTO veiling);
        Task<bool> EditVeiling(Veiling_DTO veiling);
        Task<List<Veiling_DTO>> GetVeilingen(string term, int take, bool almostFinishedVeilingen);
    }
}
