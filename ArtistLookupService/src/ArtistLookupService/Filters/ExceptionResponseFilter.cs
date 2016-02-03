using System.Net;
using Microsoft.AspNet.Mvc;
using Microsoft.AspNet.Mvc.Filters;

namespace ArtistLookupService.Filters
{
    public class ExceptionResponseFilter : ExceptionFilterAttribute
    {
        public override void OnException(ExceptionContext context)
        {
            context.Result = new JsonResult("ErrorMessage: An internal server error occurred.");
            context.HttpContext.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

            base.OnException(context);
        }
    }
}
