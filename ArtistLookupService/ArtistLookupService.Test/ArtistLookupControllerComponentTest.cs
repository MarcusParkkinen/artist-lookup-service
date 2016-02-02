using System.Collections.Generic;
using System.Net;
using ArtistLookupService.Domain;
using ArtistLookupService.Test.Extensions;
using FluentAssertions;
using Microsoft.AspNet.TestHost;
using Xunit;

namespace ArtistLookupService.Test
{
    public class ArtistLookupControllerComponentTest
    {
        private readonly TestServer _testServer;

        public ArtistLookupControllerComponentTest()
        {
            _testServer = new TestServer(TestServer.CreateBuilder().UseStartup<Startup>());
        }

        [Fact]
        public async void Get_With_Empty_Mbid_Returns_404()
        {
            using (var client = _testServer.CreateClient())
            {
                var response = await client.GetAsync("api/artistlookup");

                response.StatusCode.Should().Be(HttpStatusCode.NotFound);
            }
        }

        [Fact]
        public async void Get_With_Non_Empty_Mbid_Returns_Artist()
        {
            using (var client = _testServer.CreateClient())
            {
                const string mbid = "abc-123";
                var response = await client.GetAsync($"api/artistlookup/{mbid}");
                var result = await response.Content.ReadAsJsonAsync<Artist>();

                result.Description.Should().NotBeEmpty();
            }
        }
    }
}

