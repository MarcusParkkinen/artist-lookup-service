using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Threading;
using System.Threading.Tasks;

namespace ArtistLookupService.Test.Extensions
{
    public static class HttpContentExtensions
    {
        public static async Task<T> ReadAsJsonAsync<T>(this HttpContent httpContent)
        {
            return await httpContent.ReadAsAsync<T>(GetJsonFormatters());
        }

        private static IEnumerable<MediaTypeFormatter> GetJsonFormatters()
        {
            yield return new JsonMediaTypeFormatter();
        }
    }
}
