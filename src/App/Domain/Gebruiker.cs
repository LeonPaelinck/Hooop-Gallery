using Ardalis.GuardClauses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain {
    public class Gebruiker {
        public string Gebruikersnaam { get; private set; }
        public DateTime Geboortedatum { get; private set; }
        public string Email { get; private set; }
        public int GebruikerId { get; private set; }
        public DateTime DatumCreatie { get;  set; }
        public string FotoPad { get; set; }
        public string Details { get; set; }

        public Gebruiker(string gebruikersnaam, DateTime geboortedatum, string email, string fotoPad, string details) : this() {
            //Guard.Against.NullOrEmpty(gebruikersnaam, nameof(gebruikersnaam));
            Gebruikersnaam = Guard.Against.NullOrWhiteSpace(gebruikersnaam, nameof(gebruikersnaam));
            Geboortedatum = Guard.Against.Null(geboortedatum, nameof(geboortedatum));
            Email = Guard.Against.NullOrWhiteSpace(email, nameof(email));
            DatumCreatie = DateTime.UtcNow;
            this.FotoPad = fotoPad;
            this.Details = Guard.Against.NullOrWhiteSpace(details, nameof(details));
            //this.FotoPad = Guard.Against.NullOrEmpty(fotoPad, nameof(fotoPad));
        }

        public Gebruiker() { }

        public void Edit(string gebruikersnaam, DateTime geboortedatum/*, string email*/, string fotoPad, string details) {
            this.Gebruikersnaam = gebruikersnaam;
            this.Geboortedatum = geboortedatum;
            //this.Email = email;
            this.FotoPad = fotoPad;
            this.Details = details;
        }
    }
}
