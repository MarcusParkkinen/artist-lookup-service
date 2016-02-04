using System.Threading.Tasks;

namespace ArtistLookupService.External_Service_Interfaces
{
    public interface IDescriptionService
    {
        Task<string> GetAsync(string uri);
    }
}