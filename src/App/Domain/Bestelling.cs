using Ardalis.GuardClauses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Domain {
    public class Bestelling {
        public int Id { get; set; }
        public DateTime Datum { get; set; }
        public Address Adres { get; set; }
        //public DateTime LeverDatum { get; set; }
        public decimal TotalePrijs { get; set; }
        public string PaymentId { get; set; }
        public ICollection<Kunstwerk> WinkelmandKunstwerken { get; set; } // deze niet in DB, winkelmand lokaal bijhouden

        public Bestelling() {
            WinkelmandKunstwerken = new List<Kunstwerk>();
        }

        public Bestelling(DateTime datum, Address adres /*string straat, int postcode, string gemeente, DateTime leverDatum*/, string paymentId, decimal totalePrijs, List<Kunstwerk> kunstwerken) : this() {
            Datum = Guard.Against.Null(datum, nameof(datum));
            Adres = Guard.Against.Null(adres, nameof(adres));
            //LeverDatum = Guard.Against.Null(leverDatum, nameof(leverDatum));
            PaymentId = paymentId;
            TotalePrijs = totalePrijs;
            WinkelmandKunstwerken = kunstwerken;
        }
    }
}


