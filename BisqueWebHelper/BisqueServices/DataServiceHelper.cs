
namespace BisqueWebHelper
{
    using Tools;
    using RestSharp;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using System.Net.Mime;

    internal class DataServiceHelper
    {
        public static readonly string ContentType = "content-type";

        private const string DataServicePrefix = "data_service/";            

        internal static IRestResponse QueryWithParameter(RestClient client, string imageId, string parameter)
        {
            var request = new RestRequest(DataServicePrefix + imageId, Method.GET);
            request.AddParameter(parameter, null);
            var response = client.Execute(request);
            RestSharpHelpers.CheckResponse(response);
            return response;
        }

        internal static IRestResponse GetAllImages(RestClient client)
        {
            var request = new RestRequest(DataServicePrefix + "image", Method.GET);            
            var response = client.Execute(request);
            RestSharpHelpers.CheckResponse(response);
            return response;
        }

        /// <summary>
        /// Sets the bisque metadata / tags of a file.
        /// </summary>
        /// <param name="client">The client used for the request.</param>
        /// <param name="file">The image file.</param>
        /// <param name="metaDataXml">The meta data XML.</param>
        /// <returns>The respones of the request.</returns>
        internal static IRestResponse SetMetaData(RestClient client, BisqueImageResource file, string metaDataXml)
        {
            var request = new RestRequest(DataServicePrefix + file.Id, Method.PUT);
            request.AddHeader(ContentType, MediaTypeNames.Text.Xml);
            request.AddParameter(MediaTypeNames.Text.Xml, metaDataXml, ParameterType.RequestBody);

            var response = client.Execute(request);
            RestSharpHelpers.CheckResponse(response);
            var content = response.Content; // raw content as string

            RestSharpHelpers.CheckResponse(response);
            return response;
        }
    }
}
