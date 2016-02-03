using ArtistLookupService.External_Service_Interfaces;
using ArtistLookupService.Filters;
using ArtistLookupService.Model;
using Microsoft.AspNet.Mvc;

namespace ArtistLookupService.Controllers
{
    [Route("api/[controller]")]
    public class ArtistLookupController : Controller
    {
        private readonly IArtistDetailsService _artistDetailsService;

        public ArtistLookupController(IArtistDetailsService artistDetailsService)
        {
            _artistDetailsService = artistDetailsService;
        }

        [HttpGet("{mbid}")]
        [ServiceFilter(typeof(LogExceptionFilter))]
        [ServiceFilter(typeof(ExceptionResponseFilter))]
        public Artist Get(string mbid)
        {
            return _artistDetailsService.Get(mbid);
        }
    }
}
