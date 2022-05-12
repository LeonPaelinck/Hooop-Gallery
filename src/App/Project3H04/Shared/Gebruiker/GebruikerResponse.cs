using Project3H04.Shared.DTO;
using System;

namespace Project3H04.Shared.Gebruiker {
    public class GebruikerResponse {
        public class Detail {
            public Gebruiker_DTO Gebruiker { get; set; }
        }

        public class Edit {
            public Uri Sas { get; set; }
        }
    }
}
