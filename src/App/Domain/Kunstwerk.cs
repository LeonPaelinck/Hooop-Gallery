using Ardalis.GuardClauses;
using System;
using System.Collections;
using System.Collections.Generic;

namespace Domain {
    public class Kunstwerk {
        public int Id { get; set; }
        public string Naam { get; private set; }
        public DateTime Einddatum { get; private set; }
        public int Prijs { get; private set; }
        public decimal Lengte { get; private set; }
        public decimal Breedte { get; private set; }
        public decimal Hoogte { get; set; }
        public decimal? Gewicht { get; set; }
        public string Beschrijving { get; private set; }
        public List<Foto> Fotos { get; set; }
        public bool TeKoop { get; set; }
        public bool IsVeilbaar { get; set; }
        public string Materiaal { get; private set; }
        public Kunstenaar Kunstenaar { get; private set; }

        public Kunstwerk(string naam, DateTime einddatum, int prijs, string beschrijving, decimal lengte, decimal breedte, decimal hoogte, decimal gewicht, List<Foto> fotos, bool isVeilbaar, string materiaal, Kunstenaar kunstenaar) : this() {
            Naam = Guard.Against.NullOrWhiteSpace(naam, nameof(naam));
            Einddatum = Guard.Against.Null(einddatum, nameof(einddatum));
            Prijs = Guard.Against.Null(prijs, nameof(prijs));
            Lengte = Guard.Against.Null(lengte, nameof(lengte));
            Breedte = breedte;
            Hoogte = Guard.Against.Null(hoogte, nameof(hoogte));
            Gewicht = gewicht;
            Beschrijving = Guard.Against.NullOrWhiteSpace(beschrijving, nameof(beschrijving));
            Materiaal = Guard.Against.NullOrEmpty(materiaal, nameof(materiaal));
            Kunstenaar = Guard.Against.Null(kunstenaar, nameof(kunstenaar));

            Fotos = fotos;
            IsVeilbaar = isVeilbaar;
            TeKoop = !isVeilbaar;
        }

        public Kunstwerk() { }

        public void Edit(string naam, DateTime einddatum, int prijs, decimal lengte, decimal breedte, decimal hoogte, decimal gewicht, string beschrijving, bool isVeilbaar, string materiaal)  {
            Naam = Guard.Against.NullOrWhiteSpace(naam, nameof(naam));
            Einddatum = Guard.Against.Null(einddatum, nameof(einddatum));
            Prijs = Guard.Against.Null(prijs, nameof(prijs));
            Lengte = Guard.Against.Null(lengte, nameof(lengte));
            Breedte = Guard.Against.Null(breedte, nameof(breedte));
            Hoogte = Guard.Against.Null(hoogte, nameof(hoogte));
            Gewicht = gewicht;
            Beschrijving = Guard.Against.NullOrWhiteSpace(beschrijving, nameof(beschrijving));
            Materiaal = Guard.Against.NullOrEmpty(materiaal, nameof(materiaal));
        }
    }
}
