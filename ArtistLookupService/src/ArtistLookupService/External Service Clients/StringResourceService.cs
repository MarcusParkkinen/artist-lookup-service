using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using ArtistLookupService.Converters;
using ArtistLookupService.Logging;
using ArtistLookupService.Wrappers;
using Newtonsoft.Json;

namespace ArtistLookupService.External_Service_Clients
{
    public abstract class StringResourceService 
    {
        private readonly IHttpClientWrapper _httpClient;
        private readonly IErrorLogger _logger;

        protected StringResourceService(IHttpClientWrapper httpClient, IErrorLogger logger)
        {
            _httpClient = httpClient;
            _logger = logger;
        }

        public async Task<string> GetAsync(string resourceId)
        {
            try
            {
                var requestUri = CreateRequestUri(resourceId);
                var response = await _httpClient.GetAsync(requestUri);

                if (response.StatusCode == HttpStatusCode.OK)
                {
                    return await DeserializeContent(response);
                }

                _logger.Log($"Error: attempt to retrieve resource from URI {requestUri} failed: {response.StatusCode} - {response.ReasonPhrase}");
            }
            catch (Exception ex)
            {
                _logger.Log(ex);
            }

            return null;
        }

        public abstract string CreateRequestUri(string resourceId);
        public abstract StringContentConverter GetConverter();

        private async Task<string> DeserializeContent(HttpResponseMessage response)
        {
            var json = await response.Content.ReadAsStringAsync();

            return JsonConvert.DeserializeObject<string>(json, GetConverter());
        }
    }

    public interface IStringResourceService
    {
        Task<string> GetAsync(string resourceId);
    }
}
