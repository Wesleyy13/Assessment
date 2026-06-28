using Microsoft.Playwright;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Threading.Tasks;

namespace Assessment.ApiTesting
{
    public class TvMazeApiTests
    {
        private string BASE_URL = "https://api.tvmaze.com";

        public class ShowSearchResult
        {
            public Show show { get; set; }
            public double score { get; set; }
        }

        public class Show
        {
            public int id { get; set; }
            public string name { get; set; }
            public string url { get; set; }
        }

        [Test]
        public async Task SearchForBreakingBadAndValidateUrl()
        {
            var playwright = await Playwright.CreateAsync();
            var requestContext = await playwright.APIRequest.NewContextAsync(new APIRequestNewContextOptions
            {
                BaseURL = BASE_URL,
                IgnoreHTTPSErrors = true
            });

            // Step 1: Search for breaking bad
            var searchResponse = await requestContext.GetAsync("/search/shows", new APIRequestContextOptions
            {
                Params = new Dictionary<string, object>
                {
                    { "q", "breaking bad" }
                }
            });

            var searchBody = await searchResponse.TextAsync();

            var searchResults = JsonSerializer.Deserialize<List<ShowSearchResult>>(searchBody);
            Assert.That(searchResults.Count, Is.GreaterThan(0), "Should find breaking bad");

            // Step 2: Extract show ID from search result
            var showId = searchResults[0].show.id;

            // Step 3: Get detailed show information
            var showResponse = await requestContext.GetAsync($"/shows/{showId}");
            var showBody = await showResponse.TextAsync();

            // Step 4: Validate url contains the ID
            var show = JsonSerializer.Deserialize<Show>(showBody);
            Assert.That(show.name, Is.EqualTo("Breaking Bad"), "Show name should be Breaking Bad");
            Assert.That(show.url, Does.Contain(showId.ToString()), "URL should contain show ID");
        }
    }
}
