using ArtistLookupService.External_Service_Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace ArtistLookupService.Test.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static void ConfigureServices(this IServiceCollection services,
            IArtistDetailsService artistService,
            ICoverArtUrlService coverArtUrlService,
            IDescriptionService descriptionService)
        {
            services.AddMvc();
            services.AddInstance(artistService);
            services.AddInstance(coverArtUrlService);
            services.AddInstance(descriptionService);
        }
    }
}
