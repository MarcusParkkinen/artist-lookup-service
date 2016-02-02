using System;
using ArtistLookupService.Domain;

namespace ArtistLookupService.External_Services
{
    public class MusicBrainzArtistService : IArtistService
    {
        public MusicBrainzArtistService(IDescriptionService descriptionService, ICoverArtUrlService coverArtUrlService)
        {
            
        }

        public Artist Get(string mbid)
        {
            throw new NotImplementedException();
        }
    }
}
