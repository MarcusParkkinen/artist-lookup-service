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
using Swashbuckle.SwaggerGen;

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
            AddSwagger(services);

            services.AddDefaultServices();

            services.AddTransient<IHttpClientWrapper, HttpClientWrapper>();
            services.AddTransient<IArtistDetailsService, MusicBrainzService>();
            services.AddTransient<IDescriptionService, WikipediaDescriptionService>();
            services.AddTransient<ICoverArtUrlService, CoverArtArchiveService>();

            /*
             * In a "real world" scenario, we would instead use ILogger with a Log4net implementation or similar. Not used
             * here due to time restrictions.
             */
            services.AddTransient<IErrorLogger, ConsoleLogger>();
        }

        private static void AddSwagger(IServiceCollection services)
        {
            services.AddSwaggerGen();
            services.ConfigureSwaggerDocument(options =>
            {
                options.SingleApiVersion(new Info
                {
                    Version = "v1",
                    Title = "Artist Lookup Service",
                    Description = "A simple mashup service for artist details lookup using a MusicBrainz ID",
                    TermsOfService = "None"
                });
            });
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            app.UseDefaultConfiguration();
            app.UseStaticFiles();
            app.UseSwaggerGen();
            app.UseSwaggerUi();
        }

        // Entry point for the application.
        public static void Main(string[] args) => WebApplication.Run<Startup>(args);
    }
}
