using System.Net;
using Microsoft.AspNet.Mvc;
using Microsoft.AspNet.Mvc.Filters;

namespace ArtistLookupService.Filters
{
    public class NullFilter : ActionFilterAttribute
    {
        public override void OnActionExecuted(ActionExecutedContext context)
        {
            var responseContent = context.Result as ObjectResult;

            if (responseContent != null && responseContent.Value == null)
            {
                responseContent.StatusCode = (int) HttpStatusCode.NotFound;
            }

            base.OnActionExecuted(context);
        }
    }
}
