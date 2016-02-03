using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace ArtistLookupService.Wrappers
{
    public interface IHttpClientWrapper
    {
        Uri BaseAddress { get; set; }

        HttpRequestHeaders DefaultRequestHeaders { get; }

        Task<HttpResponseMessage> GetAsync(string uri);
    }
}
