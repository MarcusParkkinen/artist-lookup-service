using ArtistLookupService.Extensions;
using ArtistLookupService.External_Services;
using ArtistLookupService.Test.Extensions;
using Microsoft.AspNet.Builder;
using Microsoft.AspNet.TestHost;

namespace ArtistLookupService.Test.Configuration
{
    public class TestServerFactory
    {
        public static TestServer CreateTestServerWith(IArtistService artistService = null,
            ICoverArtUrlService coverArtUrlService = null,
            IDescriptionService descriptionService = null)
        {
            var builder = TestServer.CreateBuilder()
                .UseServices(services =>
                {
                    services.ConfigureServices(artistService, coverArtUrlService, descriptionService);
                })
                .UseStartup(app => app.UseDefaultConfiguration());

            return new TestServer(builder);
        }
    }
}
