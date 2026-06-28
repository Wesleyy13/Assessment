# Assessment Test Automation Project

A comprehensive test automation suite built with .NET 10, NUnit, and Playwright for UI and API testing.

## 📋 Overview

This project contains automated tests for:
- **UI Testing**: Web application testing using Playwright with Page Object Model (POM) pattern
- **API Testing**: REST API testing for OpenWeather and TVMaze APIs

## 🛠️ Technologies Used

- **.NET 10.0** - Target framework
- **NUnit 4.3.2** - Testing framework
- **Playwright 1.61.0** - Browser automation for UI tests
- **C#** - Programming language

## 📁 Project Structure

```
Assessment/
├── ApiTesting/
│   ├── OpenWeatherApiTests.cs    # Weather API tests
│   └── TvMazeApiTests.cs          # TV show API tests
└── UiTesting/
    ├── Basepages/
    │   ├── BasePOM.cs             # Base Page Object Model
    │   └── BaseTests.cs           # Base test setup
    ├── Homepage/
    │   ├── HomepagePOM.cs         # Homepage page objects
    │   └── HomepageTests.cs       # Homepage test cases
    ├── Cartpage/
    │   ├── CartPOM.cs             # Cart page objects
    │   └── CartTests.cs           # Shopping cart tests
    └── Contactpage/
        ├── ContactPOM.cs          # Contact page objects
        └── ContactTests.cs        # Contact form tests
```

## 🚀 Getting Started

### Prerequisites

- **Visual Studio 2022 or later** (Community, Professional, or Enterprise)
- **.NET 10 SDK** or later
- **PowerShell** (for Playwright installation)

### Installation Steps

1. **Clone the repository**
   ```bash
   git clone https://github.com/Wesleyy13/Assessment.git
   cd Assessment
   ```

2. **Restore NuGet packages**
   ```bash
   dotnet restore
   ```

3. **Build the project**
   ```bash
   dotnet build
   ```

4. **Install Playwright browsers**
   ```powershell
   # Navigate to the build output directory
   cd bin/Debug/net10.0

   # Run the Playwright installation script
   pwsh playwright.ps1 install
   ```

## ▶️ Running Tests

### Run all tests
```bash
dotnet test
```

### Run specific test categories

**UI Tests only:**
```bash
dotnet test --filter "FullyQualifiedName~UiTesting"
```

**API Tests only:**
```bash
dotnet test --filter "FullyQualifiedName~ApiTesting"
```

### Run from Visual Studio
1. Open `Assessment.slnx` in Visual Studio
2. Open **Test Explorer** (Test → Test Explorer)
3. Click **Run All** or select specific tests to run

## 📝 Test Coverage

### UI Tests
- Homepage functionality (product filtering, navigation)
- Shopping cart operations
- Contact form validation and submission

### API Tests
- **OpenWeather API**: Weather data retrieval and validation
- **TVMaze API**: TV show information queries

## 🔧 Configuration

- Test configurations can be found in the respective test classes
- API endpoints and credentials are defined in the test files
- Browser settings and timeouts are configured in `BaseTests.cs`

## 📖 Writing New Tests

### UI Test Example
```csharp
public class NewPageTests : BaseTests
{
    private NewPagePOM newPagePOM;

    [SetUp]
    public new async Task Setup()
    {
        await base.Setup();
        newPagePOM = new NewPagePOM(Page);
    }

    [Test]
    public async Task YourTestName()
    {
        // Your test logic here
    }
}
```

### API Test Example
```csharp
[Test]
public async Task YourApiTest()
{
    var request = await Playwright.APIRequest.NewContextAsync();
    var response = await request.GetAsync("your-api-endpoint");
    Assert.That(response.Status, Is.EqualTo(200));
}
```

## 🤝 Contributing

1. Fork the repository
2. Create a feature branch (`git checkout -b feature/new-test`)
3. Commit your changes (`git commit -m 'Add new test'`)
4. Push to the branch (`git push origin feature/new-test`)
5. Open a Pull Request

## 📄 License

This project is created for assessment purposes.

## 👤 Author

Wesley - [GitHub](https://github.com/Wesleyy13)

---

*Last updated: 2025*
