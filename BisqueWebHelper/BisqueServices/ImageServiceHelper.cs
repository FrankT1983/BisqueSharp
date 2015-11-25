namespace BisqueWebHelper
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using RestSharp;
    using Tools;

    internal class ImageServiceHelper
    {        
        internal static IRestResponse QueryWithParameter(RestClient client, string imageId, string parameterName, string parameterValue = null)
        {
            var request = new RestRequest("image_service/" + imageId, Method.GET);
            request.AddParameter(parameterName, parameterValue);
            var response = client.Execute(request);
            RestSharpHelpers.CheckResponse(response);
            return response;
        }
    }

    internal static class ImageServiceParameters
    {
        /// <summary>
        /// This parameter can be used to get the meta data embedded in the file.
        /// </summary>
        public const string MetaData = "meta";


        /// <summary>
        /// This parameter can be used for general inforamtion about the image. Like size and format.
        /// </summary>
        public const string Info = "info";

        public const string Slice = "slice";

        public static string SliceParameter(int x, int y, int z, int t)
        {
            return x + "," + y + "," + z + "," + t;
        }
    }
}
