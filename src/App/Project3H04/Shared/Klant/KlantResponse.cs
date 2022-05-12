using Project3H04.Shared.DTO;

namespace Project3H04.Shared.Klant {
    public class KlantResponse {
        public class Detail {
            public Klant_DTO Klant { get; set; }
        }

        public class Create {
            public string Message { get; set; }
        }
    }
}
