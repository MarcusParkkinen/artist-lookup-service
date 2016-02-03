using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using ArtistLookupService.Converters;
using ArtistLookupService.External_Service_Interfaces;
using ArtistLookupService.Model;
using ArtistLookupService.Wrappers;
using Newtonsoft.Json;

namespace ArtistLookupService.External_Service_Clients
{
    public class MusicBrainzService : IArtistDetailsService
    {
        private readonly IHttpClientWrapper _client;
        private const string Uri = "http://musicbrainz.org/ws/2/artist/"; // Include trailing '/'

        public MusicBrainzService(IHttpClientWrapper httpClient,
            IDescriptionService descriptionService,
            ICoverArtUrlService coverArtUrlService)
        {
            _client = httpClient;
            _client.BaseAddress = new Uri(Uri);
        }

        public Artist Get(string mbid)
        {
            return GetAsync(mbid).Result;
        }

        public async Task<Artist> GetAsync(string mbid)
        {
            var requestUri = CreateRequestUri(mbid);
            var response = await _client.GetAsync(requestUri);

            if (response.StatusCode != HttpStatusCode.OK)
            {
                return null;
            }

            var artist = await DeserializeContent(response);

            return artist;
        }

        private static async Task<Artist> DeserializeContent(HttpResponseMessage response)
        {
            var json = await response.Content.ReadAsStringAsync();

            return JsonConvert.DeserializeObject<Artist>(json, new ArtistConverter());
        }

        private static string CreateRequestUri(string mbid)
        {
            return $"{Uri}{mbid}?&inc=url-rels+release-groups";
        }
    }
}
