namespace Project3H04.Shared.Kunstwerken {
    public class KunstwerkRequest {
        public class Create {
            public Kunstwerk_DTO.Create Kunstwerk { get; set; }
        }

        public class Edit {
            public Kunstwerk_DTO.Edit Kunstwerk { get; set; }
        }
    }
}
