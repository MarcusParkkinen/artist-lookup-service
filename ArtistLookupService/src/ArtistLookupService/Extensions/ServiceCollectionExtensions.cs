using ArtistLookupService.Filters;
using Microsoft.Extensions.DependencyInjection;

namespace ArtistLookupService.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static void AddDefaultServices(this IServiceCollection services)
        {
            services.AddMvc();

            services.AddScoped<LogExceptionFilter>();
            services.AddScoped<ExceptionResponseFilter>();
            services.AddScoped<NullFilter>();
        }
    }
}
