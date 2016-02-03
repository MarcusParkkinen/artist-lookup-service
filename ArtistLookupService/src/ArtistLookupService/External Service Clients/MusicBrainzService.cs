using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using ArtistLookupService.Extensions;
using ArtistLookupService.External_Service_Interfaces;
using ArtistLookupService.Model;
using ArtistLookupService.Wrappers;

namespace ArtistLookupService.External_Service_Clients
{
    public class MusicBrainzService : IArtistDetailsService
    {
        private readonly IHttpClientWrapper _client;
        private const string Uri = "http://musicbrainz.org/ws/2/artist/";

        public MusicBrainzService(IHttpClientWrapper httpClient, IDescriptionService descriptionService, ICoverArtUrlService coverArtUrlService)
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

            return await DeserializeContent(response);
        }

        private static async Task<Artist> DeserializeContent(HttpResponseMessage response)
        {
            return await response.Content.ReadAsJsonAsync<Artist>();
        }

        private static string CreateRequestUri(string mbid)
        {
            return $"{Uri}{mbid}?&inc=url-rels+release-groups";
        }
    }
}
