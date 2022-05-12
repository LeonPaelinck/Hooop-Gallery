using Project3H04.Shared.DTO;
using Project3H04.Shared.Kunstwerken;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Project3H04.Shared.Veilingen;

namespace Project3H04.Shared.Kunstenaars {
    public class Kunstenaar_DTO: Gebruiker_DTO {
        //public string Details { get; set; }
        public bool StatusActiefKunstenaar { get; set; }
        public ICollection<Kunstwerk_DTO.Index> Kunstwerken { get; set; }
        public Abonnement_DTO Abonnement { get; set; }
        public string GeboortedatumShort { get; set; }

        //bij aanmaken van kunstenaar dan validatie met validator
    }
}
