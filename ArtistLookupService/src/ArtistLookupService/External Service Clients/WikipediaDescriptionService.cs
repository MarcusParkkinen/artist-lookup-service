using System;
using System.Linq;
using ArtistLookupService.Converters;
using ArtistLookupService.External_Service_Interfaces;
using ArtistLookupService.Logging;
using ArtistLookupService.Wrappers;

namespace ArtistLookupService.External_Service_Clients
{
    public class WikipediaDescriptionService : StringResourceService, IDescriptionService
    {
        public WikipediaDescriptionService(IHttpClientWrapper httpClient, IErrorLogger logger) : base(httpClient, logger)
        {
        }

        private const string WikipediaUri = "https://en.wikipedia.org/w/api.php?action=query&format=json&prop=extracts&exintro=true&redirects=true&titles=";

        public override string CreateRequestUri(string resourceId)
        {
            if (string.IsNullOrEmpty(resourceId))
            {
                throw new ArgumentException($"Description Uri cannot be null or empty.");
            }

            var identifier = resourceId.Split('/').Last();

            return $"{WikipediaUri}{identifier}";
        }

        public override StringContentConverter GetConverter()
        {
            return new ArtistDescriptionConverter();
        }
    }
}