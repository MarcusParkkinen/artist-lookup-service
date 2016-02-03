﻿using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using ArtistLookupService.Extensions;
using ArtistLookupService.External_Service_Interfaces;
using ArtistLookupService.Model;

namespace ArtistLookupService.External_Service_Clients
{
    public class MusicBrainzService : IArtistDetailsService
    {
        private readonly HttpClient _client;
        private const string Uri = "http://musicbrainz.org/ws/2/artist/";

        public MusicBrainzService(IDescriptionService descriptionService, ICoverArtUrlService coverArtUrlService)
        {
            _client = new HttpClient
            {
                BaseAddress = new Uri(Uri)
            };
            _client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        public Artist Get(string mbid)
        {
            return GetAsync(mbid).Result;
        }

        public async Task<Artist> GetAsync(string mbid)
        {
            var requestUri = CreateRequestUri(mbid);
            var response = await _client.GetAsync(requestUri);
            var result = await response.Content.ReadAsJsonAsync<Artist>();

            return result;
        }

        private static string CreateRequestUri(string mbid)
        {
            return $"{Uri}{mbid}?&inc=url-rels+release-groups";
        }
    }
}
