using System.Threading.Tasks;

namespace ArtistLookupService.External_Service_Interfaces
{
    public interface ICoverArtUrlService
    {
        Task<string> GetAsync(string albumId);
    }
}