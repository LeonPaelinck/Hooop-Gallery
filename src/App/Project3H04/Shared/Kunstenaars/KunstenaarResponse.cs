using System.Collections.Generic;

namespace Project3H04.Shared.Kunstenaars {
    public class KunstenaarResponse {
        public class Index {
            public List<Kunstenaar_DTO> Kunstenaars { get; set; }
        }

        public class Detail {
            public Kunstenaar_DTO Kunstenaar { get; set; }
        }
    }
}
