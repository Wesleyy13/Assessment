using System.Threading.Tasks;
using Microsoft.Playwright;
using NUnit.Framework;

namespace Assessment
{
    public class Tests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public async Task Test1()
        {
            using var playwright = await Playwright.CreateAsync();
            await using var browser = await playwright.Chromium.LaunchAsync(new BrowserTypeLaunchOptions { Headless = false });
            var page = await browser.NewPageAsync();
            await page.GotoAsync("https://example.com");
            var title = await page.TitleAsync();
            Assert.That(title, Does.Contain("Example Domain"));
        }
    }

}
