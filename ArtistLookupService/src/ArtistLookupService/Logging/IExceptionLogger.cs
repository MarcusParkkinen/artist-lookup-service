using System;
using Microsoft.AspNet.Http;

namespace ArtistLookupService.Logging
{
    public interface IExceptionLogger
    {
        void Log(HttpRequest request, Exception ex);
        void Log(Exception ex);
    }
}
