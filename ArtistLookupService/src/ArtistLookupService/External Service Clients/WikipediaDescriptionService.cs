using System;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using ArtistLookupService.Converters;
using ArtistLookupService.External_Service_Interfaces;
using ArtistLookupService.Logging;
using ArtistLookupService.Wrappers;
using Newtonsoft.Json;

namespace ArtistLookupService.External_Service_Clients
{
    public class WikipediaDescriptionService : IDescriptionService
    {
        private readonly IHttpClientWrapper _httpClient;
        private readonly IExceptionLogger _logger;

        public WikipediaDescriptionService(IHttpClientWrapper httpClient, IExceptionLogger logger)
        {
            _httpClient = httpClient;
            _logger = logger;
        }

        public async Task<string> GetAsync(string uri)
        {
            if (string.IsNullOrEmpty(uri))
            {
                return null;
            }

            try
            {
                var transformedUri = TransformUri(uri);
                var response = await _httpClient.GetAsync(transformedUri);
                var description = await DeserializeContent(response);

                return description;
            }
            catch (Exception ex)
            {
                _logger.Log(ex);
            }

            return null;
        }

        private static async Task<string> DeserializeContent(HttpResponseMessage response)
        {
            var json = await response.Content.ReadAsStringAsync();

            return JsonConvert.DeserializeObject<string>(json, new ArtistDescriptionConverter());
        }

        private static string TransformUri(string uri)
        {
            var identifier = uri.Split('/').Last();

            return $"{WikipediaUri}{identifier}";
        }

        private const string WikipediaUri = "https://en.wikipedia.org/w/api.php?action=query&format=json&prop=extracts&exintro=true&redirects=true&titles=";
    }
}