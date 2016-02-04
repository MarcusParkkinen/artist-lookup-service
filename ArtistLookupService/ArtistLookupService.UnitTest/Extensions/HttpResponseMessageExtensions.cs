using System.Net;
using System.Net.Http;

namespace ArtistLookupService.UnitTest.Extensions
{
    public static class HttpResponseMessageExtensions
    {
        public static HttpResponseMessage PopulatedWith(this HttpResponseMessage httpResponseMessage, HttpStatusCode httpStatusCode = HttpStatusCode.OK, string content = "")
        {
            httpResponseMessage.StatusCode = httpStatusCode;
            httpResponseMessage.Content = new StringContent(content);

            return httpResponseMessage;
        }
    }
}
