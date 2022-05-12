using System.Threading.Tasks;
using Microsoft.Playwright;
using Microsoft.Playwright.NUnit;
using NUnit.Framework;

namespace Project3H04.PlaywrightTests
{
    public class ArtistsTest : PageTest
    {
        private const string ServerBaseUrl = "https://localhost:5001";
        [Test]
        public async Task Filter_On_Artist_ShouldReturn1()
        {
            await using var browser = await Playwright.Chromium.LaunchAsync(new BrowserTypeLaunchOptions
            {
                Headless = false,
                SlowMo = 500
            });

            var page = await browser.NewPageAsync();
            await page.GotoAsync($"{ServerBaseUrl}/artists");
            await page.WaitForSelectorAsync("data-test-id=artists");
            await page.FillAsync("data-test-id=artistsearch", "inara");
            await page.Keyboard.PressAsync("Enter");
            await page.WaitForSelectorAsync("data-test-id=artists");
            var artists = await page.Locator("data-test-id=artists").CountAsync();
            Assert.AreEqual(1, artists);
        }
        [Test]
        public async Task HomePage_Recently_Joined_Artists_ShouldReturn4()
        {
            await using var browser = await Playwright.Chromium.LaunchAsync(new BrowserTypeLaunchOptions
            {
                Headless = false,
                SlowMo = 500
            });
            var page = await browser.NewPageAsync();
            await page.GotoAsync($"{ServerBaseUrl}/");
            await page.WaitForSelectorAsync("data-test-id=recentlyjoined");
            var recentlyJoinedArtists = await page.Locator("data-test-id=recentlyjoined").CountAsync();
            Assert.AreEqual(4, recentlyJoinedArtists);
        }
    }
}
