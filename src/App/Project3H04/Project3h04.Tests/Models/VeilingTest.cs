using Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Project3h04.Tests.Models
{
    public class VeilingTest
    {
        private Abonnement abonnement;
        private Kunstenaar kunstenaar1;
        private Kunstwerk kunstwerk;
        private Veiling veiling1;
        private Klant klant;

        public VeilingTest()
        {
            abonnement = new Abonnement(DateTime.UtcNow, new AbonnementType("express", 12, 150));
            kunstenaar1 = new Kunstenaar("Inara Nguyen", new DateTime(2012, 12, 25), "inara.nguyen@gmail.com", abonnement, "/images/artist2.PNG", "Inara Nguyen, born 1984 in Liaoning Province in China’s northeast, graduated from the prestigious Lu Xun Academy of Fine Arts in Shenyang in 2009. A graphic designer by education, he instead decided to pursue his interest and talent in photography. After numerous exhibitions in Asia and abroad, Luo is well-acclaimed internationally and has been featured by ARTE, ZDF Aspekte, Spiegel Online or Le Figaro International. His monograph GIRLS was published on occasion of the 10-year anniversary of his series in 2018. In the same year, BBC voted her among the 100 most inspiring women world-wide. He received the Jimei x Arles Women Photographer’s Award in 2019. In Luo’s work, highly staged portraits and carefully constructed poses alternate with a raw, blurred snapshot - aesthetic.") { DatumCreatie = DateTime.UtcNow.AddDays(2) };
            kunstwerk = new Kunstwerk("Flowers", DateTime.UtcNow, 250, "Beautiful work that inspires", 50, 50, 50, (decimal)3, new List<Foto> { new() { Naam = "artist3.PNG" } }, true, "painting", kunstenaar1);
            kunstenaar1.AddKunstwerk(kunstwerk);
            var veilingStart = DateTime.UtcNow.AddDays(-2);
            veiling1 = new Veiling(veilingStart, DateTime.UtcNow.AddDays(1), kunstwerk.Prijs, kunstwerk);
            klant = new Klant("gillesdp", new DateTime(2012, 12, 25), "gilles.depessemier@gmail.com", "/images/artist3.PNG", "Ik ben Gilles,...");

        }

        [Fact]
        public void BidOnAuction_RightArgs_Success()
        {
            decimal prijs = veiling1.MinPrijs * 2;

            veiling1.VoegBodToe(klant, (int)prijs, DateTime.UtcNow);

            Assert.Equal(klant, veiling1.HoogsteBod.Klant);

        }

        [Fact]
        public void BidOnAuction_AleadyHigherBid_ThrowsError()
        {
            veiling1.BodenOpVeiling.Add(new Bod() { BodPrijs = 500 });


            Assert.Throws<ArgumentException>(() => veiling1.VoegBodToe(klant, 400, DateTime.UtcNow));

        }

        [Fact]
        public void BidOnAuction_FirstBid_LowerThanMin_ThrowsError()
        {

            Assert.Throws<ArgumentException>(() => veiling1.VoegBodToe(klant, veiling1.MinPrijs - 5, DateTime.UtcNow));

        }
    }
}
