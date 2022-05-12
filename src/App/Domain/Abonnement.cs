using Ardalis.GuardClauses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain {
    public class Abonnement {
        public DateTime StartDatum { get; private set; }
        public DateTime EindDatum { get; private set; }
        public int Id { get; private set; }
        public AbonnementType AbonnementType { get; set; }

        public Abonnement() { }

        public Abonnement(DateTime startDatum, AbonnementType abonnementType) : this() {
            StartDatum = Guard.Against.Null(startDatum, nameof(startDatum));
            EindDatum = startDatum.AddMonths(abonnementType.Verlooptijd);
            this.AbonnementType = Guard.Against.Null(abonnementType, nameof(abonnementType));
        }

        public void SetBothData(DateTime startDatum) {
            StartDatum = Guard.Against.Null(startDatum, nameof(startDatum));
            EindDatum = startDatum.AddMonths(AbonnementType.Verlooptijd);
        }
    }
}
