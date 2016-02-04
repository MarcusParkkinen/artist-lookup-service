using System;
using Microsoft.AspNet.Http;

namespace ArtistLookupService.Logging
{
    public class ExceptionLogger : IExceptionLogger
    {
        public void Log(HttpRequest request, Exception ex)
        {
        }

        public void Log(Exception ex)
        {
        }

        public void Log(string message)
        {
        }
    }
}
