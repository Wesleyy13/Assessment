using Assessment.BasePages;
using NUnit.Framework;
using System;
using System.Threading.Tasks;

namespace Assessment.HomePage
{
    public class HomepageTests : BaseTests
    {
        private HomepagePOM homepagePOM;

        public HomepageTests() { }

        [SetUp]
        public new async Task Setup()
        {
            homepagePOM = new HomepagePOM(Page);
        }

        // Scenario: Verify that the homepage loads successfully and displays the correct title
        [Test]
        public async Task VerifyHomepageLoadsSuccessfully()
        {
            // Arrange
            // Expect the site title to contain the site name; use a contains assertion to be resilient to minor changes
            string expectedContains = "Practice Software Testing";
            // Act
            string actualTitle = await homepagePOM.GetPageTitleAsync();
            // Assert
            Assert.That(actualTitle, Does.Contain(expectedContains), "The homepage title does not contain the expected site name.");
        }
    }
}
