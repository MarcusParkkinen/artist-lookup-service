using System;
using Microsoft.AspNet.Http;

namespace ArtistLookupService.Logging
{
    /*
     * Note: Replace the usage of this interface with ILogger instead. This simple interface is used due to time restrictions.
     */
    public interface IErrorLogger
    {
        void Log(HttpRequest request, Exception ex);
        void Log(Exception ex);
        void Log(string message);
    }
}
