using Microsoft.Playwright.NUnit;
using NUnit.Framework;
using System;
using System.Threading.Tasks;

namespace Project3H04.PlaywrightTests
{
    public class ArtworkTests : PageTest
    {
        private const string ServerBaseUrl = "https://localhost:5001";
        [Test]
        public async Task Navigate_To_Artworks_ShouldReturn4()
        {

            await Page.GotoAsync($"{ServerBaseUrl}/artworks");
            await Page.WaitForSelectorAsync("data-test-id=kunstwerk");
            var amountOfKunstwerken = await Page.Locator("data-test-id=kunstwerk").CountAsync();

            Assert.AreEqual(4, amountOfKunstwerken); ;


        }

        [Test]
        public async Task Filter_On_Artist_ShouldReturn3()
        {

            await Page.GotoAsync($"{ServerBaseUrl}/artworks");
            await Page.WaitForSelectorAsync("data-test-id=kunstwerk");

            await Page.FillAsync("data-test-id=artistsearch","inara");
            await Page.Keyboard.PressAsync("Enter");
            await Page.WaitForSelectorAsync("data-test-id=kunstwerk");

            var amountOfKunstwerken = await Page.Locator("data-test-id=kunstwerk").CountAsync();
            Assert.AreEqual(3, amountOfKunstwerken);
                
        }
        [Test]
        public async Task Filter_On_Artworks_ShouldReturn2()
        {

            await Page.GotoAsync($"{ServerBaseUrl}/artworks");
            await Page.WaitForSelectorAsync("data-test-id=kunstwerk");

            await Page.FillAsync("data-test-id=artworksearch", "flowers");
            await Page.Keyboard.PressAsync("Enter");
            await Page.WaitForSelectorAsync("data-test-id=kunstwerk");

            var amountOfKunstwerken = await Page.Locator("data-test-id=kunstwerk").CountAsync();
            Assert.AreEqual(2, amountOfKunstwerken);
        }
        [Test]
        public async Task Filter_On_Medium_ShouldReturn2()
        {

            await Page.GotoAsync($"{ServerBaseUrl}/artworks");
            await Page.WaitForSelectorAsync("data-test-id=kunstwerk");


            await Page.UncheckAsync(".col-md-12 ul li:nth-child(3) input");
            await Page.CheckAsync("[data-test-id=\"mediumsearch\"]");
            await Page.WaitForSelectorAsync("data-test-id=kunstwerk");

            var amountOfKunstwerken = await Page.Locator("data-test-id=kunstwerk").CountAsync();
            Assert.AreEqual(2, amountOfKunstwerken);
        }
        [Test]
        public async Task Filter_On_Price_ShouldReturn1()
        {

            await Page.GotoAsync($"{ServerBaseUrl}/artworks");
            await Page.WaitForSelectorAsync("data-test-id=kunstwerk");

            // await Page.Keyboard.PressAsync("Enter");
            await Page.FillAsync("data-test-id=minPrice", "300");
            await Page.FillAsync("data-test-id=minPrice", "400");
            await Page.Keyboard.PressAsync("Enter");
            await Page.WaitForSelectorAsync("data-test-id=kunstwerk");

            var amountOfKunstwerken = await Page.Locator("data-test-id=kunstwerk").CountAsync();
            Assert.AreEqual(1, amountOfKunstwerken);
        }
    }
}