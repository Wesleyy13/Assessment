using Assessment.UiTesting.Basepages;
using Microsoft.Playwright;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Assessment.UiTesting.Cartpage
{
    public class CartPOM : BasePOM
    {
        public CartPOM(IPage page) : base(page) { }

        // Navigate to products page
        public async Task NavigateToProductsAsync()
        {
            await page.GotoAsync(BASE_ADRESS + "products");
            await page.WaitForTimeoutAsync(500);
        }

        // Navigate to cart page
        public async Task NavigateToCartAsync()
        {
            await page.Locator("a[href*='cart']").First.ClickAsync();
            await page.WaitForLoadStateAsync(LoadState.DOMContentLoaded);
        }

        // Add product to cart by index
        public async Task AddProductToCartAsync(int productIndex)
        {
            var addButtons = await page.QuerySelectorAllAsync("button:has-text('Add to cart'), button[data-testid='add-to-cart']");
            if (productIndex < addButtons.Count)
            {
                await addButtons[productIndex].ClickAsync();
                await page.WaitForTimeoutAsync(500);
            }
        }

        // Add product to cart by name
        public async Task AddProductToCartByNameAsync(string productName)
        {
            // Find product and click add to cart button
            var productLocator = page.Locator($"text={productName}");
            var productContainer = productLocator.Locator("..").Locator("..");
            var addButton = productContainer.Locator("button", new LocatorLocatorOptions { HasText = "Add to cart" });
            await addButton.First.ClickAsync();
            await page.WaitForLoadStateAsync(LoadState.DOMContentLoaded);
        }

        // Get product names in cart
        public async Task<List<string>> GetProductNamesInCartAsync()
        {
            var productElements = await page.QuerySelectorAllAsync(".cart-item-name, [data-testid='cart-product-name']");
            var productNames = new List<string>();

            foreach (var element in productElements)
            {
                var text = await element.TextContentAsync();
                if (text != null)
                    productNames.Add(text.Trim());
            }

            return productNames;
        }

        // Increase product quantity in cart
        public async Task IncreaseProductQuantityAsync(int productIndex)
        {
            var increaseButtons = await page.QuerySelectorAllAsync("button:has-text('+'), button[data-testid='increase-qty']");
            if (productIndex < increaseButtons.Count)
            {
                await increaseButtons[productIndex].ClickAsync();
                await page.WaitForTimeoutAsync(500);
            }
        }

        // Get cart item price
        public async Task<string> GetProductPriceAsync(int productIndex)
        {
            var priceElements = await page.QuerySelectorAllAsync(".cart-item-price, [data-testid='cart-product-price']");
            if (productIndex < priceElements.Count)
            {
                return await priceElements[productIndex].TextContentAsync();
            }
            return string.Empty;
        }

        // Get product quantity
        public async Task<string> GetProductQuantityAsync(int productIndex)
        {
            var qtyElements = await page.QuerySelectorAllAsync("input[data-testid='quantity'], .quantity-value");
            if (productIndex < qtyElements.Count)
            {
                return await qtyElements[productIndex].TextContentAsync();
            }
            return string.Empty;
        }

        // Check if product is out of stock
        public async Task<bool> IsProductOutOfStockAsync(int productIndex)
        {
            var outOfStockButtons = await page.QuerySelectorAllAsync("button:disabled:has-text('Out of stock'), button[data-testid='add-to-cart']:disabled");
            return outOfStockButtons.Count > productIndex;
        }

        // Remove product from cart
        public async Task RemoveProductFromCartAsync(int productIndex)
        {
            var removeButtons = page.Locator("button").Filter(new LocatorFilterOptions { HasText = "Remove" });
            await removeButtons.Nth(productIndex).ClickAsync();
            await page.WaitForLoadStateAsync(LoadState.DOMContentLoaded);
        }

        // Empty cart
        public async Task EmptyCartAsync()
        {
            var emptyCartButton = await page.QuerySelectorAsync("button:has-text('Empty cart'), button[data-testid='empty-cart']");
            if (emptyCartButton != null)
            {
                await emptyCartButton.ClickAsync();
                await page.WaitForTimeoutAsync(500);
            }
        }

        // Get message on screen
        public async Task<string> GetMessageAsync(string messageType)
        {
            // messageType could be "removed", "empty", etc.
            var messageElement = await page.QuerySelectorAsync($".alert, [data-testid='message'], .message");
            if (messageElement != null)
            {
                return await messageElement.TextContentAsync();
            }
            return string.Empty;
        }

        // Get all messages on screen
        public async Task<List<string>> GetAllMessagesAsync()
        {
            var messageElements = await page.QuerySelectorAllAsync(".alert, [data-testid='message'], .message");
            var messages = new List<string>();

            foreach (var element in messageElements)
            {
                var text = await element.TextContentAsync();
                if (text != null)
                    messages.Add(text.Trim());
            }

            return messages;
        }

        public async Task<int> GetCartItemCountAsync()
        {
            var cartItems = await page.QuerySelectorAllAsync(".cart-item, [data-testid='cart-item']");
            return cartItems.Count;
        }

        // Click on a product by name to view product details
        public async Task ClickProductByNameAsync(string productName)
        {
            await page.Locator($"text={productName}").First.ClickAsync();
            await page.WaitForLoadStateAsync(LoadState.DOMContentLoaded);
        }

        // Check if "Niet op voorraad" text is visible on product details page
        public async Task<bool> IsOutOfStockTextVisibleAsync()
        {
            var count = await page.Locator("text=Niet op voorraad").CountAsync();
            return count > 0;
        }

        // Check if empty cart message is visible
        public async Task<bool> IsEmptyCartMessageVisibleAsync()
        {
            var count = await page.Locator("text=De winkelwagen is leeg. Niets te tonen.").CountAsync();
            return count > 0;
        }
    }
}
