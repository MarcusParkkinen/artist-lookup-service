using System.Net.Http;
using System.Net.Http.Headers;

namespace ArtistLookupService.Test.Extensions
{
    public static class HttpClientExtensions
    {
        public static HttpClient AcceptJson(this HttpClient client)
        {
            /*
             * Facilitates telling the HttpClient that we only accept JSON as response format
             */
            client.DefaultRequestHeaders.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            return client;
        }
    }
}
