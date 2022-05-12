using FluentValidation;
using Project3H04.Shared.Kunstenaars;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace Project3H04.Shared.Kunstwerken {
    public static class Kunstwerk_DTO {
        public class Index { 
            public int Id { get; set; }
            public string Naam { get; set; }
            public int Prijs { get; set; }
            public string Materiaal { get; set; }
            public DateTime Einddatum { get; set; }
            public Kunstenaar_DTO Kunstenaar { get; set; }
            public Foto_DTO HoofdFoto { get; set; }
            public bool TeKoop { get; set; }
            public bool IsVeilbaar { get; set; }
        }

        public class Detail : Index {
            public decimal Lengte { get; set; }
            public decimal Breedte { get; set; }
            public decimal Hoogte { get; set; }
            public decimal Gewicht { get; set; }
            public List<Foto_DTO> Fotos { get; set; }
            public string Beschrijving { get; set; }
            //public bool TeKoop { get; set; }
            //public bool IsVeilbaar { get; set; }

            public Detail() {
                HoofdFoto = Fotos?.FirstOrDefault();
            }
        }

        public class Filter {
            public string Naam { get; set; }
            public int MinimumPrijs { get; set; }
            public int MaximumPrijs { get; set; }
            public List<string> Materiaal { get; set; }
            public List<string> Grootte { get; set; }
            public string Kunstenaar { get; set; }
            public List<string> BetaalOpties { get; set; }

            public int Page { get; set; }
            public string SortOpNaam { get; set; }
        }

        public class Create {
            public string Naam { get; set; }
            public int Prijs { get; set; }
            public List<Foto_DTO> Fotos { get; set; } = new List<Foto_DTO>();

            public string Materiaal { get; set; }
            public string Beschrijving { get; set; }
            public bool TeKoop { get; set; }
            public bool IsVeilbaar { get; set; }
            public decimal Lengte { get; set; }
            public decimal Breedte { get; set; }
            public decimal Hoogte { get; set; }
            public decimal Gewicht { get; set; }
            public DateTime Einddatum { get; set; }
            public string KunstenaarEmail { get; set; }

            public List<Foto_DTO> NieuweFotos => Fotos.Where(f => f.Uploaded == false).ToList();
        }

        public class Edit : Create  {
            public Edit() : base() { }

            public Edit(Detail kunstwerk) {
                Id = kunstwerk.Id;
                Naam = kunstwerk.Naam;
                Prijs = kunstwerk.Prijs;
                Fotos = kunstwerk.Fotos;
                Materiaal = kunstwerk.Materiaal;
                Beschrijving = kunstwerk.Beschrijving;
                KunstenaarId = kunstwerk.Kunstenaar.GebruikerId;
                TeKoop = kunstwerk.TeKoop;
                Lengte = kunstwerk.Lengte;
                Breedte = kunstwerk.Breedte;
                Hoogte = kunstwerk.Hoogte;
                Gewicht = kunstwerk.Gewicht;
                Einddatum = kunstwerk.Einddatum;
            }

            public int Id { get; set; }
            public int KunstenaarId { get; set; }

            public List<Foto_DTO> OudeFotos => Fotos.Where(f => f.Uploaded).ToList(); //reeds in blob
        }

        //bij aanmaken van kunstwerk dan validatie met validator
        //=>bij andere DTO's wnr nodig/moet aanmaken/create heeft, dan ook validatie doen !!!
        public class Validator : AbstractValidator<Create> {
            public Validator() {
                RuleFor(artwork => artwork.Naam).NotEmpty().OverridePropertyName("Name");
                RuleFor(artwork => artwork.Prijs).GreaterThanOrEqualTo(0).OverridePropertyName("Price");
                RuleFor(artwork => artwork.Materiaal).NotEmpty().OverridePropertyName("Material");
                RuleFor(artwork => artwork.Fotos).NotEmpty().OverridePropertyName("Images");
                RuleFor(artwork => artwork.Beschrijving).NotEmpty().OverridePropertyName("Description");
                RuleFor(artwork => artwork.Lengte).NotEmpty().OverridePropertyName("Length");
                RuleFor(artwork => artwork.Breedte).NotEmpty().OverridePropertyName("Width");
                RuleFor(artwork => artwork.Hoogte).NotEmpty().OverridePropertyName("Height");
                RuleFor(artwork => artwork.Gewicht).NotEmpty().OverridePropertyName("Weight");
            }
        }
    }
}
