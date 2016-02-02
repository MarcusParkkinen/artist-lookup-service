using ArtistLookupService.External_Services;
using ArtistLookupService.Test.Fakes;
using Microsoft.Extensions.DependencyInjection;

namespace ArtistLookupService.Test.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static void ConfigureServices(this IServiceCollection services,
            IArtistService artistService = null,
            ICoverArtUrlService coverArtUrlService = null,
            IDescriptionService descriptionService = null)
        {
            services.AddMvc();
            services.AddInstance(artistService ?? new FakeArtistService());
            services.AddInstance(coverArtUrlService ?? new FakeCoverArtUrlService());
            services.AddInstance(descriptionService ?? new FakeDescriptionService());
        }
    }
}
