using Project3H04.Shared.DTO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project3H04.Shared.Gebruiker {
    public class GebruikerRequest {
        public class Edit {
            public Gebruiker_DTO Model { get; set; }
            public bool newImage { get; set; }
            public string newImageName { get; set; }
        }
    }
}
