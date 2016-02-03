using System.Threading.Tasks;
using ArtistLookupService.External_Service_Interfaces;
using ArtistLookupService.Wrappers;

namespace ArtistLookupService.External_Service_Clients
{
    public class DescriptionService : IDescriptionService
    {
        private readonly IHttpClientWrapper _httpClient;

        public DescriptionService(IHttpClientWrapper httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<string> Get(string uri)
        {
            var response = await _httpClient.GetAsync(uri);
            var description = await response.Content.ReadAsStringAsync();

            return description;
        }
    }
}