using ArtistLookupService.Extensions;
using ArtistLookupService.External_Service_Clients;
using ArtistLookupService.External_Service_Interfaces;
using ArtistLookupService.Logging;
using ArtistLookupService.Wrappers;
using Microsoft.AspNet.Builder;
using Microsoft.AspNet.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace ArtistLookupService
{
    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder().AddJsonFile("appsettings.json");

            builder.AddEnvironmentVariables();

            Configuration = builder.Build().ReloadOnChanged("appsettings.json");
        }

        public IConfigurationRoot Configuration { get; set; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDefaultServices();

            services.AddTransient<IHttpClientWrapper, HttpClientWrapper>();
            services.AddTransient<IArtistDetailsService, MusicBrainzService>();
            services.AddTransient<IDescriptionService, WikipediaDescriptionService>();
            services.AddTransient<ICoverArtUrlService, CoverArtUrlService>();
            services.AddTransient<IExceptionLogger, ExceptionLogger>();
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            app.UseDefaultConfiguration();
        }

        // Entry point for the application.
        public static void Main(string[] args) => WebApplication.Run<Startup>(args);
    }
}
