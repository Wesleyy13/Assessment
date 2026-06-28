using Assessment.UiTesting.Basepages;
using Microsoft.Playwright;
using System.Threading.Tasks;

namespace Assessment.UiTesting.Contactpage
{
    public class ContactPOM : BasePOM
    {
        public ContactPOM(IPage page) : base(page) { }

        // Navigate to contact page
        public async Task NavigateToContactPageAsync()
        {
            await page.GotoAsync(BASE_ADRESS + "contact");
            await page.WaitForTimeoutAsync(500);
        }

        // Fill contact form
        public async Task FillFullNameAsync(string fullName)
        {
            await page.FillAsync("input[name='fullname'], input[name='full_name'], input[placeholder*='name'], input[id*='name']", fullName, new() { Force = true });
            await page.WaitForTimeoutAsync(300);
        }

        public async Task FillEmailAsync(string email)
        {
            await page.FillAsync("input[type='email'], input[name='email'], input[placeholder*='email']", email, new() { Force = true });
            await page.WaitForTimeoutAsync(300);
        }

        public async Task FillSubjectAsync(string subject)
        {
            // Subject might not exist on the form, try multiple selectors
            try
            {
                await page.FillAsync("input[name='subject'], input[placeholder*='subject']", subject, new() { Force = true });
            }
            catch
            {
                // If subject field doesn't exist, skip it
                Console.WriteLine("Subject field not found, skipping...");
            }
            await page.WaitForTimeoutAsync(300);
        }

        public async Task FillMessageAsync(string message)
        {
            await page.FillAsync("textarea[name='message'], textarea[placeholder*='message'], textarea[name*='message']", message, new() { Force = true });
            await page.WaitForTimeoutAsync(300);
        }

        // Submit form
        public async Task SubmitFormAsync()
        {
            try
            {
                var submitButton = await page.QuerySelectorAsync("button[type='submit'], button:has-text('Submit'), button:has-text('Send')");
                if (submitButton != null)
                {
                    await submitButton.ClickAsync();
                }
                else
                {
                    await page.PressAsync("textarea", "Enter");
                }
            }
            catch
            {
                await page.ClickAsync("button");
            }
            await page.WaitForTimeoutAsync(1000);
        }

        // Get success message
        public async Task<string> GetSuccessMessageAsync()
        {
            var successElement = await page.QuerySelectorAsync(".alert-success");
            if (successElement != null)
            {
                return await successElement.TextContentAsync();
            }
            return string.Empty;
        }

        public async Task<string> GetCurrentUrlAsync()
        {
            return page.Url;
        }

        // Get validation error messages
        public async Task<bool> HasFullNameErrorAsync()
        {
            var errorElement = await page.QuerySelectorAsync("input[id='full-name']:invalid");
            return errorElement != null;
        }

        public async Task<bool> HasEmailErrorAsync()
        {
            var errorElement = await page.QuerySelectorAsync("input[id='email']:invalid");
            return errorElement != null;
        }

        public async Task<bool> HasSubjectErrorAsync()
        {
            var errorElement = await page.QuerySelectorAsync("input[id='subject']:invalid");
            return errorElement != null;
        }

        public async Task<bool> HasMessageErrorAsync()
        {
            var errorElement = await page.QuerySelectorAsync("textarea[id='message']:invalid");
            return errorElement != null;
        }

        public async Task<string> GetFullNameErrorMessageAsync()
        {
            var errorElement = await page.QuerySelectorAsync(".invalid-feedback-full-name, [data-field='full-name'] .invalid-feedback");
            if (errorElement != null)
            {
                return await errorElement.TextContentAsync();
            }
            return string.Empty;
        }

        public async Task<string> GetEmailErrorMessageAsync()
        {
            var errorElement = await page.QuerySelectorAsync(".invalid-feedback-email, [data-field='email'] .invalid-feedback, .error-email");
            if (errorElement != null)
            {
                return await errorElement.TextContentAsync();
            }
            return string.Empty;
        }

        public async Task<string> GetMessageErrorMessageAsync()
        {
            var errorElement = await page.QuerySelectorAsync(".invalid-feedback-message, [data-field='message'] .invalid-feedback, .error-message");
            if (errorElement != null)
            {
                return await errorElement.TextContentAsync();
            }
            return string.Empty;
        }
    }
}
