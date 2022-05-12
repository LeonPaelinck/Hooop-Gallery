using Ardalis.GuardClauses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain {
    public class Veiling {
        public int Id { get; private set; }
        public DateTime StartDatum { get; set; }
        public DateTime EindDatum { get; set; }
        public int MinPrijs { get; set; }
        public ICollection<Bod> BodenOpVeiling { get; set; }
        public Kunstwerk Kunstwerk { get; private set; }
        public int KunstwerkId { get; private set; }
        public Bod HoogsteBod => BodenOpVeiling.OrderByDescending(b => b.BodPrijs).FirstOrDefault();


        public Veiling(DateTime startDatum, DateTime eindDatum, int minPrijs, Kunstwerk kunstwerk) : this() {
            StartDatum = Guard.Against.Null(startDatum, nameof(startDatum));
            EindDatum = Guard.Against.Null(eindDatum, nameof(eindDatum));
            MinPrijs = Guard.Against.Null(minPrijs, nameof(MinPrijs));
            Kunstwerk = Guard.Against.Null(kunstwerk, nameof(kunstwerk));
            KunstwerkId = kunstwerk.Id;
            Kunstwerk.IsVeilbaar = true;
        }

        public void VoegBodToe(Klant klant, int prijs, DateTime datum) {
            var bod = new Bod(klant, prijs, datum);

            if ((BodenOpVeiling.Count > 0) && (prijs <= HoogsteBod.BodPrijs))
                throw new ArgumentException("De prijs van het bod ligt onder de prijs van het hoogste bod.");
            if ((prijs <= MinPrijs) || (datum > EindDatum))
                throw new ArgumentException("De prijs of datum van het bod is onjuist.");

            BodenOpVeiling.Add(bod);
        }

        public Veiling() {
            BodenOpVeiling = new List<Bod>();
        }
    }
}
