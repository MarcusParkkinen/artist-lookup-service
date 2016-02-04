using System;
using ArtistLookupService.Converters;
using ArtistLookupService.External_Service_Interfaces;
using ArtistLookupService.Logging;
using ArtistLookupService.Wrappers;

namespace ArtistLookupService.External_Service_Clients
{
    public class CoverArtArchiveService : StringResourceService, ICoverArtUrlService
    {
        public CoverArtArchiveService(IHttpClientWrapper httpClient, IErrorLogger logger) : base(httpClient, logger)
        {
        }

        private const string CoverArtArchiveUrl = "http://coverartarchive.org/release-group/"; // Include trailing '/'

        public override string CreateRequestUri(string resourceId)
        {
            if (string.IsNullOrEmpty(resourceId))
            {
                throw new ArgumentException($"Album ID cannot be null or empty.");
            }

            return $"{CoverArtArchiveUrl}{resourceId}";
        }

        public override StringContentConverter GetConverter()
        {
            return new CoverArtUrlConverter();
        }
    }
}