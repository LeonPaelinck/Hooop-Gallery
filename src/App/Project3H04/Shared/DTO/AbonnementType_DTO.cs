using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Project3H04.Shared.DTO {
    public class AbonnementType_DTO {
        public string Naam { get; set; }
        public int Verlooptijd { get; set; }
        public double Prijs { get; set; }
    }
}
