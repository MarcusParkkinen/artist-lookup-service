﻿using ArtistLookupService.Extensions;
using ArtistLookupService.External_Service_Interfaces;
using ArtistLookupService.Logging;
using Microsoft.AspNet.TestHost;
using Microsoft.Extensions.DependencyInjection;

namespace ArtistLookupService.Test.Configuration
{
    public class TestServerFactory
    {
        public static TestServer CreateTestServerWith(IArtistDetailsService artistService,
            ICoverArtUrlService coverArtUrlService,
            IDescriptionService descriptionService,
            IExceptionLogger exceptionLogger)
        {
            var builder = TestServer.CreateBuilder().UseServices(services =>
                {
                    services.AddDefaultServices();

                    services.AddInstance(artistService);
                    services.AddInstance(coverArtUrlService);
                    services.AddInstance(descriptionService);
                    services.AddInstance(exceptionLogger);
                })
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
