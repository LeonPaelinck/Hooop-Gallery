using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain {
    public class Foto : ValueObject {
        private static readonly string defaultLocatie = "https://devopsh04storage.blob.core.windows.net/fotos/images/";

        public int Id { get; set; }
        public string Pad => Path.Combine(Locatie, Naam);

        public string Naam { get; set; }
        public string Locatie { get; set; }

        public Foto() {
            Locatie = defaultLocatie;
        }

        public Foto(string naam, string locatie) : this() {
            Naam = naam;
            Locatie = locatie;
        }

        protected override IEnumerable<object> GetEqualityComponents() {
            yield return Id;
        }
    }
}
