using Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Project3h04.Tests.Models
{
   public class KunstwerkTest
    {
        [Fact]
        public void NewProduct_ValidAndOptionalData_CreatesProduct()
        {
            AbonnementType at = new AbonnementType("default", 3, 200);
            Abonnement abonnement2 = new Abonnement(DateTime.UtcNow, at);
            Kunstenaar kunstenaar2 = new Kunstenaar("Issac Ellis", DateTime.UtcNow, "issac.ellis@gmail.com", abonnement2, "A1.png", "Throughout his sculptures, installations, photography, and paintings, Liu Bolin explores the tensions between individualism and collectivism, particularly in his native China. In his most famous series, “Hiding in the City” (also known as “Invisible Man”), the photographer stands immobile, perfectly painted in camouflage to blend into detailed backgrounds that range from magazine stands to the Great Wall. With these compositions, Bolin comments on consumerism, rapid development, and the role of the artist in contemporary Chinese society. He has exhibited in New York, London, Paris, Beijing, Stockholm, and Hong Kong, and has given performances at the Hirshhorn Museum and Sculpture Garden, the Centre Pompidou, and Art Basel in Miami Beach. Bolin’s work belongs in the collections of the Baltimore Museum of Art, Fotografiska, the M+ Sigg Collection, the Museo Enzo Ferrari, and the Red Mansion Foundation.") { DatumCreatie = DateTime.UtcNow.AddDays(3) };
            Kunstwerk kunstwerk1 = new Kunstwerk("Inspiring Flowers", DateTime.UtcNow, 200, "Beautiful work that inspires", 40, 40, 40, (decimal)1.50, new List<Foto> { new() { Naam = "A1AT1.png" } }, false, "painting", kunstenaar2);
            Assert.Equal("Inspiring Flowers", kunstwerk1.Naam);
            Assert.Equal("Issac Ellis", kunstwerk1.Kunstenaar.Gebruikersnaam);
            Assert.Equal(200, kunstwerk1.Prijs);
            Assert.False(kunstwerk1.IsVeilbaar);
        }
    }
}
