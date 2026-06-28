using Assessment.UiTesting.Basepages;
using Microsoft.Playwright;
using System.Threading.Tasks;

namespace Assessment.UiTesting.Contactpage
{
    public class ContactPOM : BasePOM
    {
        public ContactPOM(IPage page) : base(page) { }

        // Navigate to contact page by clicking Contact tab
        public async Task NavigateToContactPageAsync()
        {
            // Click on the Contact link in the top navigation bar (top right)
            await page.ClickAsync("[data-test='nav-contact']");
            await page.WaitForLoadStateAsync(LoadState.NetworkIdle);
        }

        // Fill first name
        public async Task FillFirstNameAsync(string firstName)
        {
            await page.FillAsync("[data-test='first-name']", firstName);
            await page.WaitForTimeoutAsync(300);
        }

        // Fill last name
        public async Task FillLastNameAsync(string lastName)
        {
            await page.FillAsync("[data-test='last-name']", lastName);
            await page.WaitForTimeoutAsync(300);
        }

        // Fill email
        public async Task FillEmailAsync(string email)
        {
            await page.FillAsync("[data-test='email']", email);
            await page.WaitForTimeoutAsync(300);
        }

        // Select subject from dropdown
        public async Task SelectSubjectAsync(string subject)
        {
            await page.SelectOptionAsync("[data-test='subject']", subject);
            await page.WaitForTimeoutAsync(300);
        }

        // Fill message
        public async Task FillMessageAsync(string message)
        {
            await page.FillAsync("[data-test='message']", message);
            await page.WaitForTimeoutAsync(300);
        }

        // Submit form
        public async Task SubmitFormAsync()
        {
            await page.ClickAsync("[data-test='contact-submit']");
            await page.WaitForTimeoutAsync(1000);
        }

        // Get success message
        public async Task<string> GetSuccessMessageAsync()
        {
            var successElement = await page.QuerySelectorAsync(".alert-success, [data-test='success-message']");
            if (successElement != null)
            {
                return await successElement.TextContentAsync() ?? string.Empty;
            }
            return string.Empty;
        }

        // Validation error checks
        public async Task<bool> HasFirstNameErrorAsync()
        {
            var errorElement = await page.QuerySelectorAsync("[data-test='first-name-error'], #first-name-error");
            return errorElement != null;
        }

        public async Task<bool> HasLastNameErrorAsync()
        {
            var errorElement = await page.QuerySelectorAsync("[data-test='last-name-error'], #last-name-error");
            return errorElement != null;
        }

        public async Task<bool> HasEmailErrorAsync()
        {
            var errorElement = await page.QuerySelectorAsync("[data-test='email-error'], #email-error");
            return errorElement != null;
        }

        public async Task<bool> HasMessageErrorAsync()
        {
            var errorElement = await page.QuerySelectorAsync("[data-test='message-error'], #message-error");
            return errorElement != null;
        }

        public async Task<string> GetCurrentUrlAsync()
        {
            return page.Url;
        }
    }
}
