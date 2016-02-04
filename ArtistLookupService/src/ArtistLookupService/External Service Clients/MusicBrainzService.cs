﻿using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using ArtistLookupService.Converters;
using ArtistLookupService.External_Service_Interfaces;
using ArtistLookupService.Logging;
using ArtistLookupService.Model;
using ArtistLookupService.Wrappers;
using Newtonsoft.Json;

namespace ArtistLookupService.External_Service_Clients
{
    public class MusicBrainzService : IArtistDetailsService
    {
        private readonly IHttpClientWrapper _client;
        private readonly IDescriptionService _descriptionService;
        private readonly IErrorLogger _logger;

        private const string Uri = "http://musicbrainz.org/ws/2/artist/"; // Include trailing '/'

        public MusicBrainzService(IHttpClientWrapper httpClient,
            IDescriptionService descriptionService,
            ICoverArtUrlService coverArtUrlService,
            IErrorLogger logger)
        {
            _client = httpClient;
            _descriptionService = descriptionService;
            _logger = logger;
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
                _logger.Log($"Error: attempt to retrieve artist details from MusicBrainz failed: {response.StatusCode} - {response.ReasonPhrase}");
                return null;
            }

            var artist = await DeserializeContent(response);

            PopulateAdditionalDetails(artist);

            return artist;
        }

        private void PopulateAdditionalDetails(Artist artist)
        {
            var populateDescriptionTask = PopulateDescription(artist);
            //var populateAlbumCoversTask = PopulateAlbumCovers(artist);

            Task.WaitAll(populateDescriptionTask);
        }

        private static async Task PopulateAlbumCovers(Artist artist)
        {
            throw new NotImplementedException();
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

        private async Task PopulateDescription(Artist artist)
        {
            var description = await _descriptionService.GetAsync(artist.WikipediaUri);

            artist.Description = description;
        }
    }
}
