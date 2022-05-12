using Ardalis.GuardClauses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain {
    public class Bod {
        public int Id { get; private set; }
        public Klant Klant { get; private set; }
        public int BodPrijs { get; set; }
        public DateTime Datum { get; set; }
        public int KlantId { get; private set; }

        public Bod(Klant klant, int bodPrijs, DateTime datum) : this() {
            Klant = Guard.Against.Null(klant, nameof(klant));
            KlantId = klant.GebruikerId;
            BodPrijs = Guard.Against.Null(bodPrijs, nameof(bodPrijs));
            Datum = Guard.Against.Null(datum, nameof(datum));
        }

        public Bod() { }
    }
}

