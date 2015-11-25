
namespace BisqueWebHelper
{
    using Tools;
    using RestSharp;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;


    internal class DataServiceHelper
    {
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
    }
}
