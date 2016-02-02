using Microsoft.AspNet.Builder;

namespace ArtistLookupService.Extensions
{
    public static class ApplicationConfigurationExtensions
    {
        public static void UseDefaultConfiguration(this IApplicationBuilder app)
        {
            app.UseIISPlatformHandler();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=ArtistLookup}/{mbid?}");
            });
        }
    }
}
