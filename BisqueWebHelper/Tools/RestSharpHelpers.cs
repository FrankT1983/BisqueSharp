using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BisqueWebHelper.Tools
{
    internal static class RestSharpHelpers
    {
        /// <summary>
        /// Checks if the response of an request was ok.
        /// From <see cref="https://github.com/restsharp/RestSharp/wiki/Recommended-Usage"/>.
        /// </summary>
        /// <param name="response">The response to check.</param>        
        /// <exception cref="ApplicationException">Response contained an error.</exception>
        public static void CheckResponse(IRestResponse response)
        {
            if (response.ErrorException != null)
            {
                const string message = "Error retrieving response.  Check inner details for more info.";
                var exception = new ApplicationException(message, response.ErrorException);
                throw exception;
            }
        }
    }
}
