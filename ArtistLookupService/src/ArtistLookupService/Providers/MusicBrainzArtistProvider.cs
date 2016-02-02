using ArtistLookupService.Domain;

namespace ArtistLookupService.Providers
{
    public class MusicBrainzArtistProvider : IArtistProvider
    {
        public MusicBrainzArtistProvider(IDescriptionProvider descriptionProvider, ICoverArtUrlProvider coverArtUrlProvider)
        {
            
        }

        public Artist Get(string mbid)
        {
            return new Artist()
            {
                Description = "Music brainz artist!"
            };
        }
    }
}
