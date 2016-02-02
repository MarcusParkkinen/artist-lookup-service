using ArtistLookupService.Extensions;
using ArtistLookupService.External_Service_Interfaces;
using ArtistLookupService.Test.Extensions;
using Microsoft.AspNet.TestHost;

namespace ArtistLookupService.Test.Configuration
{
    public class TestServerFactory
    {
        public static TestServer CreateTestServerWith(IArtistDetailsService artistService,
            ICoverArtUrlService coverArtUrlService,
            IDescriptionService descriptionService)
        {
            var builder = TestServer.CreateBuilder()
                .UseServices(services => services.ConfigureServices(artistService, coverArtUrlService, descriptionService))
                .UseStartup(app => app.UseDefaultConfiguration());

            return new TestServer(builder);
        }

        public static TestServer CreateTestServer(IArtistDetailsService artistService = null,
            ICoverArtUrlService coverArtUrlService = null,
            IDescriptionService descriptionService = null)
        {
            var builder = TestServer.CreateBuilder().UseStartup<Startup>();

            return new TestServer(builder);
        }
    }
}
