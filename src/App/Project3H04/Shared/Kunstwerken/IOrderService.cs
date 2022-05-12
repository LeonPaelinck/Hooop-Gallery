using Project3H04.Shared.DTO;
using Project3H04.Shared.Order;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project3H04.Shared.Kunstwerken {
   public interface IOrderService {
       public IList<Kunstwerk_DTO.Detail> CartKunstwerken { get; set; }
       public IList<Kunstwerk_DTO.Detail> GetCartKunstwerken();

       public void AddKunstwerk(Kunstwerk_DTO.Detail kunstwerk);
       public void RemoveKunstwerk(Kunstwerk_DTO.Detail kunstwerk);

       public Task<int> PostOrderAsync(Bestelling_DTO.Create bestelling);
       public Task RemoveBestelling(string id);
       //string getBestelling(string bestellingId);
        
       bool Bestellingexists(int id);
       public Task PutOrderAsync(string id, int bestellingId);
       Task<OrderResponse.Detail> GetUserOrders(string email);
       Task<Bestelling_DTO.Index> GetBestelling(int id);
       //public Task CreateBestelling();
   }
}
