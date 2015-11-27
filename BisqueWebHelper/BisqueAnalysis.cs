namespace BisqueWebHelper
{
    using Tools;
    using RestSharp;
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;
    using System.Net.Mime;
    using System.Text;
    using System.Threading.Tasks;

    [DebuggerDisplay("Analysis {this.Name}")]
    public class BisqueAnalysis
    {
        public string Name { get; private set; }

        public BisqueAnalysis(string moduleName)
        {
            this.Name = moduleName;
        }

        public void ExecuteOnImage(BisqueSession session, BisqueImage file)
        {
            const string ModuleServicePrefix = "module_service/";
            string ContentType = "content-type";

            string parametersXml = "<mex><tag name = \"inputs\" ><tag type = \"image\" name = \"image_url\" value = \"" + file.URI +"\" /><tag type = \"system-input\" name = \"mex_url\" /><tag type = \"system-input\" name = \"bisque_token\" /></tag><tag name = \"execute_options\" ><tag name = \"iterable\" value = \"image_url\" /></tag></mex>";

            var request = new RestRequest(ModuleServicePrefix + this.Name + "/execute", Method.PUT);
            request.AddHeader(ContentType, MediaTypeNames.Text.Xml);
            request.AddParameter(MediaTypeNames.Text.Xml, parametersXml, ParameterType.RequestBody);

            var response = session.Client.Execute(request);
            RestSharpHelpers.CheckResponse(response);                                           
        }
    }
}
