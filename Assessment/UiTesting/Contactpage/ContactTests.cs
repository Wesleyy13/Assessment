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
            string firstName = "John";
            string lastName = "Doe";
            string email = "john@example.com";
            string subject = "customer-service"; // Valid subject option
            string message = "This is a test message for the contact form that is longer than 50 characters.";

            // Step 2: Fill in the contact form fields
            await contactPOM.FillFirstNameAsync(firstName);
            await contactPOM.FillLastNameAsync(lastName);
            await contactPOM.FillEmailAsync(email);
            await contactPOM.SelectSubjectAsync(subject);
            await contactPOM.FillMessageAsync(message);

            // Step 3: Submit the form
            await contactPOM.SubmitFormAsync();

            // Step 4: Verify success message is displayed
            string successMessage = await contactPOM.GetSuccessMessageAsync();
            Assert.That(successMessage, Is.Not.Empty, "Success message should be displayed after submitting form");
        }

        [Test]
        public async Task SubmitEmptyFormShowsMandatoryFieldErrors()
        {
            // Step 1: Form is already empty, proceed to submit
            await contactPOM.SubmitFormAsync();

            // Step 2: Check validation errors for required fields
            bool firstNameHasError = await contactPOM.HasFirstNameErrorAsync();
            bool lastNameHasError = await contactPOM.HasLastNameErrorAsync();
            bool emailHasError = await contactPOM.HasEmailErrorAsync();
            bool messageHasError = await contactPOM.HasMessageErrorAsync();

            // Step 3: Verify all required fields show validation errors
            Assert.That(firstNameHasError, Is.True, "First Name field should show validation error");
            Assert.That(lastNameHasError, Is.True, "Last Name field should show validation error");
            Assert.That(emailHasError, Is.True, "Email field should show validation error");
            Assert.That(messageHasError, Is.True, "Message field should show validation error");
        }

        [Test]
        public async Task InvalidEmailFormatShowsError()
        {
            // Step 1: Prepare test data with invalid email
            string firstName = "John";
            string lastName = "Doe";
            string invalidEmail = "invalid-email";
            string subject = "customer-service";
            string message = "This is a test message for the contact form that is longer than 50 characters to meet the requirement.";

            // Step 2: Fill the form with invalid email
            await contactPOM.FillFirstNameAsync(firstName);
            await contactPOM.FillLastNameAsync(lastName);
            await contactPOM.FillEmailAsync(invalidEmail);
            await contactPOM.SelectSubjectAsync(subject);
            await contactPOM.FillMessageAsync(message);

            // Step 3: Submit the form
            await contactPOM.SubmitFormAsync();

            // Step 4: Verify email validation error is displayed
            bool emailHasError = await contactPOM.HasEmailErrorAsync();
            Assert.That(emailHasError, Is.True, "Email field should show validation error for invalid format");
        }

        [Test]
        public async Task MessageLessThan50CharactersShowsError()
        {
            // Step 1: Prepare test data with short message
            string firstName = "John";
            string lastName = "Doe";
            string email = "john@example.com";
            string subject = "customer-service";
            string shortMessage = "This is too short"; // Less than 50 characters

            // Step 2: Fill the form with short message
            await contactPOM.FillFirstNameAsync(firstName);
            await contactPOM.FillLastNameAsync(lastName);
            await contactPOM.FillEmailAsync(email);
            await contactPOM.SelectSubjectAsync(subject);
            await contactPOM.FillMessageAsync(shortMessage);

            // Step 3: Submit the form
            await contactPOM.SubmitFormAsync();

            // Step 4: Verify message length validation error is displayed
            bool messageHasError = await contactPOM.HasMessageErrorAsync();
            Assert.That(messageHasError, Is.True, "Message field should show validation error for too short message");
        }
    }
}
