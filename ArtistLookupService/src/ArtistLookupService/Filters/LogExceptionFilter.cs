using ArtistLookupService.Logging;
using Microsoft.AspNet.Mvc.Filters;

namespace ArtistLookupService.Filters
{
    public class LogExceptionFilter : ExceptionFilterAttribute
    {
        private readonly IErrorLogger _errorLogger;

        public LogExceptionFilter(IErrorLogger errorLogger)
        {
            _errorLogger = errorLogger;
        }

        public override void OnException(ExceptionContext context)
        {
            _errorLogger.Log(context.HttpContext.Request, context.Exception);

            base.OnException(context);
        }
    }
}
