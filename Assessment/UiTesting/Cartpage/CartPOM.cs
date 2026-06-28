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
            try
            {
                await page.ClickAsync("a[href*='cart'], button[data-testid='cart'], .cart-link");
            }
            catch
            {
                // Try alternative selectors
                await page.ClickAsync("text=cart, text=Cart");
            }
            await page.WaitForTimeoutAsync(1000);
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
            try
            {
                // Try to find the product card/container with the product name and click its add to cart button
                var productCard = await page.QuerySelectorAsync($"xpath=//div[contains(., '{productName}')]//button[contains(text(), 'Add to cart')]");
                if (productCard != null)
                {
                    await productCard.ClickAsync();
                    await page.WaitForTimeoutAsync(500);
                    return;
                }

                // Alternative approach: find product by name in product title/heading and then find the closest add button
                var productElement = await page.QuerySelectorAsync($"xpath=//h2[contains(text(), '{productName}')] | //h3[contains(text(), '{productName}')] | //span[contains(text(), '{productName}')]");
                if (productElement != null)
                {
                    // Find the parent product container
                    var productContainer = await productElement.EvaluateAsync<IElementHandle>("el => el.closest('.product, .product-item, [data-testid=\"product\"]')");
                    if (productContainer != null)
                    {
                        var addButton = await productContainer.QuerySelectorAsync("button:has-text('Add to cart'), button[data-testid='add-to-cart']");
                        if (addButton != null)
                        {
                            await addButton.ClickAsync();
                            await page.WaitForTimeoutAsync(500);
                        }
                    }
                }
            }
            catch
            {
                // If product not found, throw exception
                throw new Exception($"Product '{productName}' not found or cannot be added to cart");
            }
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
            var removeButtons = await page.QuerySelectorAllAsync("button:has-text('Remove'), button[data-testid='remove-from-cart']");
            if (productIndex < removeButtons.Count)
            {
                await removeButtons[productIndex].ClickAsync();
                await page.WaitForTimeoutAsync(500);
            }
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
    }
}
