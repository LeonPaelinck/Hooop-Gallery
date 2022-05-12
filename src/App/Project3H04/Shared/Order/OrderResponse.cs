using Project3H04.Shared.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project3H04.Shared.Order {
    public static class OrderResponse {
        public class Detail {
           public IEnumerable<Bestelling_DTO.Index> Bestellingen { get; set; }
        }
    }
}
