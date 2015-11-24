namespace BisqueWebTest
{
    using RestSharp;
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.IO;
    using System.Xml;

    /// <summary>
    /// Simple class for using bisque.
    /// </summary>
    public class BisqueSession
    {
        private RestClient client;

        public BisqueSession(Uri serverUri)
        {
            this.client = new RestClient(serverUri);
            client.CookieContainer = new System.Net.CookieContainer();
        }

        public BisqueSession(string serverUri)
            : this(new Uri(serverUri))
        {                                  
        }

        /// <summary>
        /// Logins this instance.
        /// This will currently use the Demo-VM-Credentials.
        /// </summary>
        /// <returns> True, if login was succesfull. </returns>
        public bool Login()
        {
            var request = new RestRequest("auth_service/login_handler", Method.POST);
            request.AddParameter("login", "bisque"); // adds to POST or URL querystring based on Method
            request.AddParameter("password", "bisque"); // adds to POST or URL querystring based on Method
            var response = client.Execute(request);           

            return ( response.ResponseUri!=null && response.ResponseUri.OriginalString.Contains("client_service"));
        }

        /// <summary>
        /// Gets all the images belonging to the logged-in user.
        /// </summary>
        /// <returns>The list of images.</returns>
        public IEnumerable<BisqueImageResource> GetImages()
        {
            var requestForImages = new RestRequest("data_service/image", Method.GET);
            var response = client.Execute(requestForImages);
            var content = response.Content; // raw content as string

            return BisqueXmlHelper.ImagesFromXml(content);
        }

        /// <summary>
        /// Uploads the image in the specified path to the server.
        /// </summary>
        /// <param name="path">The path.</param>
        public void Upload(string path)
        {
            var request = new RestRequest("import/transfer", Method.POST);            
            request.AddFile("receipt[receipt_file]", File.ReadAllBytes(path), Path.GetFileName(path), "application/octet-stream");
            var response = client.Execute(request);
            var content = response.Content; // raw content as string
        }

        public string GetEmbeddedMetaData(BisqueImageResource file)
        {
            Debug.WriteLine(file.Id);
            var request = new RestRequest("image_service/"+ file.Id, Method.GET);
            request.AddParameter("meta",null);
            var response = client.Execute(request);
            var content = response.Content; // raw content as string
            return content;

        }

        public string GetMetaData(BisqueImageResource file)
        {            
            var request = new RestRequest("data_service/" + file.Id +"/tag", Method.GET);            
            var response = client.Execute(request);
            var content = response.Content; // raw content as string

            Debug.WriteLine(content);
            return content;
        }

        public string GetImageStandardData(BisqueImageResource file)
        {
            var request = new RestRequest("data_service/" + file.Id, Method.GET);
            var response = client.Execute(request);
            var content = response.Content; // raw content as string

            Debug.WriteLine(content);
            return content;
        }

        public string SetMetaData(BisqueImageResource file, string metaDataXml)
        {
            var request = new RestRequest("data_service/" + file.Id, Method.PUT);                       
            request.AddHeader("content-type", "text/xml");
            request.AddParameter("text/xml", metaDataXml, ParameterType.RequestBody);
                       
            var response = client.Execute(request);
            var content = response.Content; // raw content as string

            Debug.WriteLine(response.StatusDescription + " " + content);
            return content;
        }

        public string GetFileData(BisqueImageResource file)
        {
            var request = new RestRequest("image_service/" + file.Id, Method.GET);
            var response = client.Execute(request);
            var content = response.Content; // raw content as string

            Debug.WriteLine(content);
            return content;
        }

        public XmlDocument GetXmlDocumentForTagManupulation(BisqueImageResource file)
        {
            var xml = this.GetImageStandardData(file);
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(xml);

            // Add allready exitiong Tags
            AddExitingTagsToXml(file, xmlDoc);

            return xmlDoc;
        }

        private void AddExitingTagsToXml(BisqueImageResource file, XmlDocument xmlDoc)
        {
            var metaData = this.GetMetaData(file);
            var tags = BisqueXmlHelper.TagsFromXml(metaData);
            BisqueXmlHelper.WriteTagsToXml(xmlDoc, tags);
        }
    }
    
    public struct BisqueImageResource
    {
        public string ImageName;
        public string URI;
        public string Id;
    }
}
