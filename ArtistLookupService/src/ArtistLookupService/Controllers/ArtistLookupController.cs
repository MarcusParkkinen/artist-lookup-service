using ArtistLookupService.Domain;
using ArtistLookupService.External_Services;
using Microsoft.AspNet.Mvc;

namespace ArtistLookupService.Controllers
{
    [Route("api/[controller]")]
    public class ArtistLookupController : Controller
    {
        private readonly IArtistService _artistService;

        public ArtistLookupController(IArtistService artistService)
        {
            _artistService = artistService;
        }

        [HttpGet("{mbid}")]
        public Artist Get(string mbid)
        {
            return _artistService.Get(mbid);
        }
    }
}
