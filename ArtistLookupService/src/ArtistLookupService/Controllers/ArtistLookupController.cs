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
        [ServiceFilter(typeof(NullFilter))]
        [ResponseCache(Duration = 3600)]
        public Artist Get(string mbid)
        {
            /*
             *  Server-side caching would be recommended here, such as using a memory cache
             * that stores previously requested Artists. Not currently implemented due to
             *  time restrictions.
             */
            return _artistDetailsService.Get(mbid);
        }
    }
}
