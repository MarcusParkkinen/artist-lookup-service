using System.Net;
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
            if (string.IsNullOrEmpty(mbid))
            {
                Response.StatusCode = (int) HttpStatusCode.NotFound;
                return null;
            }

            return _artistService.Get(mbid);
        }
    }
}
