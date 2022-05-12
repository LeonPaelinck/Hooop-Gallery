using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project3H04.Shared.DTO {
    public class Klant_DTO : Gebruiker_DTO {
        //public ICollection<Bod_DTO> Boden { get; set; }
        public ICollection<Bestelling_DTO.Index> Bestellingen { get; set; }
    }
}
