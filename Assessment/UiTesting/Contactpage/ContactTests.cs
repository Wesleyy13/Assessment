using Assessment.UiTesting.Basepages;
using NUnit.Framework;
using System;
using System.Threading.Tasks;
using Assessment.UiTesting.Contactpage;

namespace Assessment.UiTesting.Contactpage
{
    public class ContactTests : BaseTests
    {
        private ContactPOM contactPOM;

        [SetUp]
        public new async Task Setup()
        {
            await base.Setup();
            contactPOM = new ContactPOM(Page);
            await contactPOM.NavigateToContactPageAsync();
        }

        [Test]
        public async Task FillAndSubmitContactForm()
        {
            // Step 1: Prepare test data
            string fullName = "John Doe";
            string email = "john@example.com";
            string message = "This is a test message for the contact form that is longer than 50 characters.";

            // Step 2: Fill in the contact form fields
            await contactPOM.FillFullNameAsync(fullName);
            await contactPOM.FillEmailAsync(email);
            await contactPOM.FillMessageAsync(message);

            // Step 3: Submit the form
            await contactPOM.SubmitFormAsync();

            // Step 4: Verify success message is displayed
            string successMessage = await contactPOM.GetSuccessMessageAsync();
            Assert.That(successMessage, Is.Not.Empty, "Success message should be displayed after submitting form");

            Console.WriteLine($"Form submitted successfully!");
            Console.WriteLine($"Success Message: {successMessage}");
        }

        [Test]
        public async Task SubmitEmptyFormShowsMandatoryFieldErrors()
        {
            // Step 1: Form is already empty, proceed to submit
            await contactPOM.SubmitFormAsync();

            // Step 2: Check validation errors for required fields
            bool fullNameHasError = await contactPOM.HasFullNameErrorAsync();
            bool emailHasError = await contactPOM.HasEmailErrorAsync();
            bool messageHasError = await contactPOM.HasMessageErrorAsync();

            // Step 3: Verify all required fields show validation errors
            Assert.That(fullNameHasError, Is.True, "Full Name field should show validation error");
            Assert.That(emailHasError, Is.True, "Email field should show validation error");
            Assert.That(messageHasError, Is.True, "Message field should show validation error");

            Console.WriteLine("All required fields show validation errors!");
            Console.WriteLine($"Full Name Error: {fullNameHasError}");
            Console.WriteLine($"Email Error: {emailHasError}");
            Console.WriteLine($"Message Error: {messageHasError}");
        }

        [Test]
        public async Task InvalidEmailFormatShowsError()
        {
            // Step 1: Prepare test data with invalid email
            string fullName = "John Doe";
            string invalidEmail = "@@##$$%%";
            string message = "This is a test message for the contact form that is longer than 50 characters to meet the requirement.";

            // Step 2: Fill the form with invalid email
            await contactPOM.FillFullNameAsync(fullName);
            await contactPOM.FillEmailAsync(invalidEmail);
            await contactPOM.FillMessageAsync(message);

            // Step 3: Submit the form
            await contactPOM.SubmitFormAsync();

            // Step 4: Verify email validation error is displayed
            string emailErrorMessage = await contactPOM.GetEmailErrorMessageAsync();
            Assert.That(emailErrorMessage, Does.Contain("E-mailformaat is ongeldig"), "Should show email format error message");

            Console.WriteLine($"Email validation error displayed: {emailErrorMessage}");
        }

        [Test]
        public async Task MessageLessThan50CharactersShowsError()
        {
            // Step 1: Prepare test data with short message
            string fullName = "John Doe";
            string email = "john@example.com";
            string shortMessage = "This is too short"; // Less than 50 characters

            // Step 2: Fill the form with short message
            await contactPOM.FillFullNameAsync(fullName);
            await contactPOM.FillEmailAsync(email);
            await contactPOM.FillMessageAsync(shortMessage);

            // Step 3: Submit the form
            await contactPOM.SubmitFormAsync();

            // Step 4: Verify message length validation error is displayed
            string messageErrorMessage = await contactPOM.GetMessageErrorMessageAsync();
            Assert.That(messageErrorMessage, Does.Contain("Bericht moet minimaal 50 tekens lang zijn"), "Should show minimum character requirement error");

            Console.WriteLine($"Message length validation error displayed: {messageErrorMessage}");
            Console.WriteLine($"Message length: {shortMessage.Length} characters (minimum required: 50)");
        }
    }
}
