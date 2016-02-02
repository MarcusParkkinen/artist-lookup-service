using System.Net;
using ArtistLookupService.Domain;
using Microsoft.AspNet.Mvc;

namespace ArtistLookupService.Controllers
{
    [Route("api/[controller]")]
    public class ArtistLookupController : Controller
    {
        [HttpGet("{mbid}")]
        public Artist Get(string mbid)
        {
            if (string.IsNullOrEmpty(mbid))
            {
                Response.StatusCode = (int) HttpStatusCode.NotFound;
                return null;
            }

            return new Artist()
            {
                Description = "Some description"
            };
        }
    }
}
