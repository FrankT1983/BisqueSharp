namespace BisqueWebHelper
{
    using Tools;
    using RestSharp;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    internal class ModuleServiceHelper
    {
        private const string ModuleServicePrefix = "module_service/";

        internal static IRestResponse GetAllModules(RestClient client)
        {
            var request = new RestRequest(ModuleServicePrefix , Method.GET);
            var response = client.Execute(request);
            RestSharpHelpers.CheckResponse(response);
            return response;
        }

    }
}
