using ArtistLookupService.Domain;
using ArtistLookupService.External_Services;

namespace ArtistLookupService.Test.Fakes
{
    public class FakeArtistService : IArtistService
    {
        public Artist Artist { get; set; }

        public FakeArtistService()
        {
            Artist = new Artist
            {
                Description = "Fake artist"
            };
        }

        public Artist Get(string mbid)
        {
            return Artist;
        }
    }
}
