using System.IO;

namespace Project3H04.Shared.Kunstwerken {
    public class Foto_DTO {
        private static readonly string defaultLocatie = "https://devopsh04storage.blob.core.windows.net/fotos/images/";
        public int Id { get; set; }
        public string Naam { get; set; }
        public string Locatie { get; set; }
        public bool Uploaded { get; set; }
        public bool PendingDelete { get; set; }

        public string Pad => Path.Combine(Locatie ?? defaultLocatie, Naam);

        public Foto_DTO() {
            Uploaded = false;
            PendingDelete = false;
        }

        public Foto_DTO(string naam) : this() {
            Naam = naam;
        }

        public Foto_DTO(string naam, string locatie) : this(naam) {
            Locatie = locatie;
        }
    }
}
