using System;
using System.Net;
using ArtistLookupService.External_Service_Interfaces;
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
        public Artist Get(string mbid)
        {
            try
            {
                return _artistDetailsService.Get(mbid);
            }
            catch (Exception)
            {
                Response.StatusCode = (int) HttpStatusCode.InternalServerError;
            }

            return null;
        }
    }
}
