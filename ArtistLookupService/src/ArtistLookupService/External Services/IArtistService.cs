using System.Threading.Tasks;
using ArtistLookupService.Domain;

namespace ArtistLookupService.External_Services
{
    public interface IArtistService
    {
        Artist Get(string mbid);
        Task<Artist> GetAsync(string mbid);
    }
}
