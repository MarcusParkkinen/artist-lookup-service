using System.Net;
using ArtistLookupService.Domain;
using ArtistLookupService.Providers;
using Microsoft.AspNet.Mvc;

namespace ArtistLookupService.Controllers
{
    [Route("api/[controller]")]
    public class ArtistLookupController : Controller
    {
        private readonly IArtistProvider _artistProvider;

        public ArtistLookupController(IArtistProvider artistProvider)
        {
            _artistProvider = artistProvider;
        }

        [HttpGet("{mbid}")]
        public Artist Get(string mbid)
        {
            if (string.IsNullOrEmpty(mbid))
            {
                Response.StatusCode = (int) HttpStatusCode.NotFound;
                return null;
            }

            return _artistProvider.Get(mbid);
        }
    }
}
