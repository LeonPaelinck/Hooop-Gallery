namespace Project3H04.Shared.Kunstenaars {
    public class KunstenaarRequest {
        public class Index {
            public string Term { get; set; }
            public int Take { get; set; }
            public bool RecentArtists { get; set; }

            public Index() {
                Term = "";
                Take = 25;
                RecentArtists = false;
            }
        }

        public class Detail {
            public int Id { get; set; }

            public Detail(int id) {
                Id = id;
            }
        }
    }
}
