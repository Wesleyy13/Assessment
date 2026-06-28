using Microsoft.Playwright;
using NUnit.Framework;
using System;
using System.Threading.Tasks;

namespace Assessment.UiTesting.Basepages
{
    // Manage Playwright lifecycle here so tests can control headed/keep-open via env vars or runsettings
    public class BaseTests
    {
        protected IPlaywright PlaywrightInstance = null!;
        protected IBrowser Browser = null!;
        protected IPage Page = null!;
        public BasePOM basePOM = null!;

        [SetUp]
        public async Task Setup()
        {
            // Read environment variables to control headed mode
            var headedEnv = Environment.GetEnvironmentVariable("PLAYWRIGHT_HEADED");
            var headed = !string.IsNullOrEmpty(headedEnv) && (headedEnv == "1" || headedEnv.Equals("true", StringComparison.OrdinalIgnoreCase));

            PlaywrightInstance = await Playwright.CreateAsync();
            Browser = await PlaywrightInstance.Chromium.LaunchAsync(new BrowserTypeLaunchOptions { Headless = !headed });
            var context = await Browser.NewContextAsync();
            Page = await context.NewPageAsync();

            basePOM = new BasePOM(Page);
            await basePOM.NavigateToUrlAsync();
        }

        [TearDown]
        public async Task TearDownAsync()
        {
            var keepOpenEnv = Environment.GetEnvironmentVariable("PLAYWRIGHT_KEEP_OPEN");
            var keepOpen = !string.IsNullOrEmpty(keepOpenEnv) && (keepOpenEnv == "1" || keepOpenEnv.Equals("true", StringComparison.OrdinalIgnoreCase));

            if (!keepOpen)
            {
                if (Browser != null)
                    await Browser.CloseAsync();
                PlaywrightInstance?.Dispose();
            }
            else
            {
                // Leave browser open so user can inspect it; write a message for clarity
                Console.WriteLine("PLAYWRIGHT_KEEP_OPEN is set; browser will remain open. Close it manually to exit tests.");
            }
        }
    }
}
