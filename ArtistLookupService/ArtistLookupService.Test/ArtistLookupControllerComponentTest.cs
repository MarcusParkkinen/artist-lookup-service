using System.Collections.Generic;
using ArtistLookupService.Test.Extensions;
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
        public async void Get_Returns_Value()
        {
            using (var client = _testServer.CreateClient().AcceptJson())
            {
                var response = await client.GetAsync("api/artistlookup");
                var result = await response.Content.ReadAsJsonAsync<List<string>>();

                Assert.NotNull(result);
                Assert.NotEmpty(result);
            }
        }
    }
}

