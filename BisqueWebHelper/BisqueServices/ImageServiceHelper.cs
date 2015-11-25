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
        internal static IRestResponse QueryWithParameter(RestClient client, string imageId, string parameter)
        {
            var request = new RestRequest("image_service/" + imageId, Method.GET);
            request.AddParameter(parameter, null);
            var response = client.Execute(request);
            RestSharpHelpers.CheckResponse(response);
            return response;
        }
    }

    internal class ImageServiceParameters
    {
        /// <summary>
        /// This parameter can be used to get the meta data embedded in the file.
        /// </summary>
        public const string MetaData = "meta";


        /// <summary>
        /// This parameter can be used for general inforamtion about the image. Like size and format.
        /// </summary>
        public const string Info = "info";
    }
}
