using System.Threading.Tasks;
using ArtistLookupService.Model;

namespace ArtistLookupService.External_Service_Interfaces
{
    public interface IArtistDetailsService
    {
        Artist Get(string mbid);
        Task<Artist> GetAsync(string mbid);
    }
}
