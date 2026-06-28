using Microsoft.Playwright;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Threading.Tasks;

namespace Assessment.ApiTesting
{
    public class OpenWeatherApiTests
    {
        private string BASE_URL = "https://api.openweathermap.org/data/2.5";
        private string GEO_URL = "http://api.openweathermap.org/geo/1.0";
        private string SOLAR_URL = "https://api.openweathermap.org/energy/2.0";
        private string appId = "18dc2ccb7c55012cdf53af5b8e1fec9a";
        private string lat = "44.34";
        private string lon = "10.99";
        private string query = "Utrecht";
        private string limit = "5";
        private string interval = "1h";

        public class WeatherResponse
        {
            public string name { get; set; }
            public Main main { get; set; }
        }

        public class Main
        {
            public double temp { get; set; }
        }

        public class ErrorResponse
        {
            public int cod { get; set; }
            public string message { get; set; }
        }

        public class GeocodingResponse
        {
            public int id { get; set; }
            public string name { get; set; }
            public double lat { get; set; }
            public double lon { get; set; }
            public string country { get; set; }
        }

        public class SolarIrradianceResponse
        {
            public List<SolarData> result { get; set; }
        }

        public class SolarData
        {
            public long timestamp { get; set; }
            public double irradiance { get; set; }
        }

        [Test]
        public async Task GetWeatherForUtrecht()
        {
            IAPIRequestContext requestContext = null;
            try
            {
                var playwright = await Playwright.CreateAsync();
                requestContext = await playwright.APIRequest.NewContextAsync(new APIRequestNewContextOptions
                {
                    IgnoreHTTPSErrors = true
                });

                // Make API request to get weather data for Utrecht
                var response = await requestContext.GetAsync($"{BASE_URL}/weather?lat={lat}&lon={lon}&appid={appId}");

                // Verify the response is successful
                Assert.That(response.Status, Is.EqualTo(200), "Response status should be 200 OK");

                // Parse the response body
                var responseBody = await response.TextAsync();

                // Validate weather data
                var weatherData = JsonSerializer.Deserialize<WeatherResponse>(responseBody);
                Assert.That(weatherData.name, Is.Not.Empty, "City name should be present");
                Assert.That(weatherData.main.temp, Is.GreaterThan(0), "Temperature should be present");
            }
            finally
            {
                if (requestContext != null)
                {
                    await requestContext.DisposeAsync();
                }
            }
        }

        [Test]
        public async Task MissingAppIdReturns401()
        {
            IAPIRequestContext requestContext = null;
            try
            {
                var playwright = await Playwright.CreateAsync();
                requestContext = await playwright.APIRequest.NewContextAsync(new APIRequestNewContextOptions
                {
                    IgnoreHTTPSErrors = true
                });

                // Make API request without appid to test error handling
                var response = await requestContext.GetAsync($"{BASE_URL}/weather?lat={lat}&lon={lon}");

                var responseBody = await response.TextAsync();

                Assert.That(response.Status, Is.EqualTo(401), "Should return 401 when appid is missing");

                var errorData = JsonSerializer.Deserialize<ErrorResponse>(responseBody);
                Assert.That(errorData.message, Is.Not.Empty, "Error message should be present");
            }
            finally
            {
                if (requestContext != null)
                {
                    await requestContext.DisposeAsync();
                }
            }
        }

        [Test]
        public async Task SearchUtrechtLocation()
        {
            IAPIRequestContext requestContext = null;
            try
            {
                var playwright = await Playwright.CreateAsync();
                requestContext = await playwright.APIRequest.NewContextAsync(new APIRequestNewContextOptions
                {
                    IgnoreHTTPSErrors = true
                });

                // Make API request to search for Utrecht location
                var response = await requestContext.GetAsync($"{GEO_URL}/direct?q={query}&limit={limit}&appid={appId}");

                // Verify the response is successful
                Assert.That(response.Status, Is.EqualTo(200), "Response status should be 200 OK");

                // Parse the response body
                var responseBody = await response.TextAsync();

                // Deserialize the array response
                var geoData = JsonSerializer.Deserialize<List<GeocodingResponse>>(responseBody);

                // Validate that we found results
                Assert.That(geoData, Is.Not.Empty, "Results should be found");

                // Validate that we found Utrecht-related results
                var utrechtResult = geoData.FirstOrDefault(x => x.name.Contains("Utrecht"));
                Assert.That(utrechtResult, Is.Not.Null, "Should find Utrecht in results");
                Assert.That(utrechtResult.name, Does.Contain("Utrecht"), "Name should contain Utrecht");
            }
            finally
            {
                if (requestContext != null)
                {
                    await requestContext.DisposeAsync();
                }
            }
        }

        [Test]
        public async Task MissingCityParameterReturns400()
        {
            IAPIRequestContext requestContext = null;
            try
            {
                var playwright = await Playwright.CreateAsync();
                requestContext = await playwright.APIRequest.NewContextAsync(new APIRequestNewContextOptions
                {
                    IgnoreHTTPSErrors = true
                });

                // Make API request without city parameter to test error handling
                var response = await requestContext.GetAsync($"{BASE_URL}/weather?q=&appid={appId}");

                // Verify the response returns 400 Bad Request
                Assert.That(response.Status, Is.EqualTo(400), "Response status should be 400 Bad Request when city is missing");
            }
            finally
            {
                if (requestContext != null)
                {
                    await requestContext.DisposeAsync();
                }
            }
        }

        [TestCase("Amsterdam")]
        [TestCase("Rotterdam")]
        [TestCase("Den Haag")]
        [TestCase("Groningen")]
        [Test]
        public async Task ValidateCityIds(string cityName)
        {
            IAPIRequestContext requestContext = null;
            try
            {
                var playwright = await Playwright.CreateAsync();
                requestContext = await playwright.APIRequest.NewContextAsync(new APIRequestNewContextOptions
                {
                    IgnoreHTTPSErrors = true
                });

                // Make API request to get city data
                var response = await requestContext.GetAsync($"{GEO_URL}/direct?q={cityName}&limit=1&appid={appId}");

                // Verify the response is successful
                Assert.That(response.Status, Is.EqualTo(200), "Response status should be 200 OK");

                // Parse the response body
                var responseBody = await response.TextAsync();

                // Deserialize the array response
                var geoData = JsonSerializer.Deserialize<List<GeocodingResponse>>(responseBody);

                // Validate that we found results
                Assert.That(geoData, Is.Not.Empty, $"Results should be found for {cityName}");

                // Get the first result
                var cityData = geoData.FirstOrDefault();

                // Validate that city name contains the search term
                Assert.That(cityData.name, Does.Contain(cityName), $"City name should contain {cityName}");

                Console.WriteLine($"City: {cityName}, Name: {cityData.name}, Country: {cityData.country}");
            }
            finally
            {
                if (requestContext != null)
                {
                    await requestContext.DisposeAsync();
                }
            }
        }
    }
}
