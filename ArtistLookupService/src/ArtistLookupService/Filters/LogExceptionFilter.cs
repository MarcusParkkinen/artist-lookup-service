using ArtistLookupService.Logging;
using Microsoft.AspNet.Mvc.Filters;

namespace ArtistLookupService.Filters
{
    public class LogExceptionFilter : ExceptionFilterAttribute
    {
        private readonly IExceptionLogger _exceptionLogger;

        public LogExceptionFilter(IExceptionLogger exceptionLogger)
        {
            _exceptionLogger = exceptionLogger;
        }

        public override void OnException(ExceptionContext context)
        {
            _exceptionLogger.Log(context.HttpContext.Request, context.Exception);

            base.OnException(context);
        }
    }
}
