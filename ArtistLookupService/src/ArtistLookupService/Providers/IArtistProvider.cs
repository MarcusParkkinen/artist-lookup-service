using ArtistLookupService.Domain;

namespace ArtistLookupService.Providers
{
    public interface IArtistProvider
    {
        Artist Get(string mbid);
    }
}
