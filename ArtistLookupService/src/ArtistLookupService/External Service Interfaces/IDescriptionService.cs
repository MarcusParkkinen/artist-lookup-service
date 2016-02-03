using System.Threading.Tasks;

namespace ArtistLookupService.External_Service_Interfaces
{
    public interface IDescriptionService
    {
        Task<string> Get(string uri);
    }
}