using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace ArtistLookupService.Wrappers
{
    public class HttpClientWrapper : IHttpClientWrapper
    {
        private readonly HttpClient _client;

        public Uri BaseAddress { get; set; }

        public HttpRequestHeaders DefaultRequestHeaders => _client.DefaultRequestHeaders;

        public HttpClientWrapper()
        {
            _client = new HttpClient();
        }

        public Task<HttpResponseMessage> GetAsync(string uri)
        {
            return _client.GetAsync(uri);
        }
    }
}