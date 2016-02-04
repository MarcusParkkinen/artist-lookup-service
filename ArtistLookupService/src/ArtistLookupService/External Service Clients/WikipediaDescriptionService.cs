using System;
using System.Linq;
using System.Net;
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
        private const string WikipediaUri = "https://en.wikipedia.org/w/api.php?action=query&format=json&prop=extracts&exintro=true&redirects=true&titles=";

        private readonly IHttpClientWrapper _httpClient;
        private readonly IErrorLogger _logger;

        public WikipediaDescriptionService(IHttpClientWrapper httpClient, IErrorLogger logger)
        {
            _httpClient = httpClient;
            _logger = logger;
        }

        public async Task<string> GetAsync(string uri)
        {
            try
            {
                var transformedUri = TransformUri(uri);
                var response = await _httpClient.GetAsync(transformedUri);

                if (response.StatusCode == HttpStatusCode.OK)
                {
                    return await DeserializeContent(response);
                }

                _logger.Log($"Error: attempt to retrieve artist description from Wikipedia failed: {response.StatusCode} - {response.ReasonPhrase}");
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
            if (string.IsNullOrEmpty(uri))
            {
                throw new ArgumentException($"Description Uri cannot be null or empty.");
            }

            var identifier = uri.Split('/').Last();

            return $"{WikipediaUri}{identifier}";
        }
    }
}