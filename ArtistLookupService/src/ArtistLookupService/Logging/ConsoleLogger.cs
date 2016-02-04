using System;
using Microsoft.AspNet.Http;

namespace ArtistLookupService.Logging
{
    public class ConsoleLogger : IErrorLogger
    {
        /*
         *  Very simple logger that logs to the VS Debug output window. Requires the debugger to be attached to work!
         */

        public void Log(HttpRequest request, Exception ex)
        {
            /*
             *  Simplification due to time restrictions.
             */
            Log(ex);
        }

        public void Log(Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"### Error caught in ConsoleLogger: {ex.Message}");
        }

        public void Log(string message)
        {
            System.Diagnostics.Debug.WriteLine($"### {message}");
        }
    }
}
