using Assessment.UiTesting.Basepages;
using Microsoft.Playwright;
using System.Threading.Tasks;

namespace Assessment.UiTesting.Homepage
{
    public class HomepagePOM : BasePOM
    {
        public HomepagePOM(IPage page) : base(page) { }

        // Price Range Filter (Scrollbar)
        public async Task SetMinPriceAsync(string minPrice)
        {
            try
            {
                var minSlider = await page.QuerySelectorAsync("input[name*='price'], input[type='range']:first-of-type");
                if (minSlider != null)
                {
                    await minSlider.FillAsync(minPrice);
                    await page.WaitForTimeoutAsync(500);
                }
            }
            catch
            {
                // If slider method fails, try with attribute selector
                await page.FillAsync("input[placeholder*='min'], input[aria-label*='min']", minPrice, new() { Force = true });
            }
        }

        public async Task SetMaxPriceAsync(string maxPrice)
        {
            try
            {
                var maxSlider = await page.QuerySelectorAsync("input[type='range']:last-of-type, input[name*='price-max']");
                if (maxSlider != null)
                {
                    await maxSlider.FillAsync(maxPrice);
                    await page.WaitForTimeoutAsync(500);
                }
            }
            catch
            {
                // If slider method fails, try with attribute selector
                await page.FillAsync("input[placeholder*='max'], input[aria-label*='max']", maxPrice, new() { Force = true });
            }
        }

        // Search
        public async Task SearchAsync(string searchTerm)
        {
            try
            {
                await page.FillAsync("input[placeholder*='search'], input[type='search'], input[name*='search']", searchTerm, new() { Force = true });
                await page.WaitForTimeoutAsync(300);
                var searchButton = await page.QuerySelectorAsync("button:has-text('Search'), button[type='submit']");
                if (searchButton != null)
                {
                    await searchButton.ClickAsync();
                }
                else
                {
                    await page.PressAsync("input[placeholder*='search'], input[type='search']", "Enter");
                }
                await page.WaitForTimeoutAsync(500);
            }
            catch
            {
                // Fallback
                await page.PressAsync("input", "Enter");
            }
        }

        // Category Filter
        public async Task SelectCategoryAsync(string categoryName)
        {
            try
            {
                var categoryLink = await page.QuerySelectorAsync($"text={categoryName}");
                if (categoryLink != null)
                {
                    await categoryLink.ClickAsync();
                    await page.WaitForTimeoutAsync(500);
                }
                else
                {
                    // Try alternative selector
                    await page.ClickAsync($"a:has-text('{categoryName}'), button:has-text('{categoryName}')");
                    await page.WaitForTimeoutAsync(500);
                }
            }
            catch
            {
                // Try by role
                await page.ClickAsync($"[role='link']:has-text('{categoryName}')");
            }
        }

        // Pagination
        public async Task GoToPageAsync(int pageNumber)
        {
            try
            {
                await page.ClickAsync($"a:has-text('{pageNumber}'), button:has-text('{pageNumber}')");
                await page.WaitForTimeoutAsync(500);
            }
            catch
            {
                var pageLink = await page.QuerySelectorAsync($"[aria-label*='page {pageNumber}'], [data-page='{pageNumber}']");
                if (pageLink != null)
                {
                    await pageLink.ClickAsync();
                    await page.WaitForTimeoutAsync(500);
                }
            }
        }

        // Product Click
        public async Task ClickFirstProductAsync()
        {
            try
            {
                var productLink = await page.QuerySelectorAsync("a[href*='product-detail'], a[href*='/product/'], .product-link");
                if (productLink != null)
                {
                    await productLink.ClickAsync();
                    await page.WaitForTimeoutAsync(500);
                }
                else
                {
                    // Fallback: click first product card
                    await page.ClickAsync("div[class*='product-']:first-child a, article >> nth=0 >> a");
                }
            }
            catch
            {
                await page.ClickAsync("[class*='product']");
            }
        }

        // Get visible products count
        public async Task<int> GetProductCountAsync()
        {
            var products = await page.QuerySelectorAllAsync("[class*='product'], article, div[class*='item']");
            return products.Count;
        }

        public async Task<string> GetCurrentUrlAsync()
        {
            return page.Url;
        }
    }
}
