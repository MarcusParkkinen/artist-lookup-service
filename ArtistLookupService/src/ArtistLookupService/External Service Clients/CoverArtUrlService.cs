using System.Threading.Tasks;
using ArtistLookupService.External_Service_Interfaces;
using ArtistLookupService.Logging;

namespace ArtistLookupService.External_Service_Clients
{
    public class CoverArtUrlService : ICoverArtUrlService
    {
        private readonly IErrorLogger _logger;

        public CoverArtUrlService(IErrorLogger logger)
        {
            _logger = logger;
        }

        public Task<string> GetAsync(string id)
        {
            return null;
        }
    }
}