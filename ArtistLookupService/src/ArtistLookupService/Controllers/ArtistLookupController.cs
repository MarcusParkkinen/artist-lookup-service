using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNet.Mvc;

namespace ArtistLookupService.Controllers
{
    [Route("api/[controller]")]
    public class ArtistLookupController : Controller
    {
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new List<string>{ "Artist lookup" };
        }
    }
}
