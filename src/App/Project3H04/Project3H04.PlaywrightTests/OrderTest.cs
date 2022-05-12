using Microsoft.Playwright;
using Microsoft.Playwright.NUnit;
using NUnit.Framework;
using System;
using System.Threading.Tasks;


namespace Project3H04.PlaywrightTests
{
    public class OrderTest : PageTest
    {
        private const string ServerBaseUrl = "https://localhost:5001";
        [Test]
        public async Task Add_Artwork_To_Cart_Returns_1_CartItem()
        {
            await using var browser = await Playwright.Chromium.LaunchAsync(new BrowserTypeLaunchOptions
            {
                Headless = false,
                SlowMo = 500
            });

            var page = await browser.NewPageAsync();


            await page.GotoAsync($"{ServerBaseUrl}");

            /* login */

            await page.RunAndWaitForNavigationAsync(async () =>
            {
                await page.ClickAsync("data-test-id=login");
            });

            await page.FillAsync("[placeholder=\"Enter your email\"]", "test@gmail.com");

            await page.FillAsync("[placeholder=\"Enter your password\"]", "test123");

            await page.RunAndWaitForNavigationAsync(async () =>
             {
                 await page.ClickAsync("text=Log In");

             });
            /* login */

           // await page.WaitForSelectorAsync("data-test-id=username");

            await page.GotoAsync($"{ServerBaseUrl}/artworks");

            await page.ClickAsync("data-test-id=kunstwerk");

            await page.WaitForSelectorAsync("h1");
            await page.ClickAsync("data-test-id=orderbutton");
            await page.IsDisabledAsync("data-test-id=orderbutton");


            await page.ClickAsync("data-test-id=cartlink");

            await page.WaitForSelectorAsync("data-test-id=cartitem");
            var length = await page.Locator("data-test-id=cartitem").CountAsync();
            Assert.AreEqual(1, length);
        }
        [Test]
        public async Task RemoveCartItem_Returns0_CartItems()
        {
            await using var browser = await Playwright.Chromium.LaunchAsync(new BrowserTypeLaunchOptions
            {
                Headless = false,
                SlowMo = 500
            });


            var page = await browser.NewPageAsync();
            await page.GotoAsync($"{ServerBaseUrl}");

            /* login */

            await page.RunAndWaitForNavigationAsync(async () =>
            {
                await page.ClickAsync("data-test-id=login");
            });
            await page.FillAsync("[placeholder=\"Enter your email\"]", "test@gmail.com");
            await page.FillAsync("[placeholder=\"Enter your password\"]", "test123");
            await page.RunAndWaitForNavigationAsync(async () =>
            {
                await page.ClickAsync("text=Log In");

            });
            /* login */

            //await page.WaitForSelectorAsync("data-test-id=username");

            /* add item to cart*/
            await page.GotoAsync($"{ServerBaseUrl}/artworks");
            await page.ClickAsync("data-test-id=kunstwerk");
            await page.WaitForSelectorAsync("h1");
            await page.ClickAsync("data-test-id=orderbutton");
            await page.IsDisabledAsync("data-test-id=orderbutton");
            await page.ClickAsync("data-test-id=cartlink");
            /* add item to cart*/

            await page.WaitForSelectorAsync("data-test-id=cartitem");
            await page.ClickAsync("data-test-id=removebutton");
            var length = await page.Locator("data-test-id=cartitem").CountAsync();
 
            Assert.AreEqual(0, length);
        }
    }

   
}
