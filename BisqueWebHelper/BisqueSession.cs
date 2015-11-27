namespace BisqueWebHelper
{  
    using RestSharp;
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.IO;
    using System.Xml;
    using Tools;

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

        public RestClient Client
        {
            get
            {
                return this.client;
            }
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
            RestSharpHelpers.CheckResponse(response);

            return ( response.ResponseUri!=null && response.ResponseUri.OriginalString.Contains("client_service"));
        }

        /// <summary>
        /// Gets all the images belonging to the logged-in user.
        /// </summary>
        /// <returns>The list of images.</returns>
        public IEnumerable<BisqueImage> GetImages()
        {
            var response = DataServiceHelper.GetAllImages(this.client);
            return BisqueImage.ImagesFromResources(BisqueXmlHelper.ImagesFromXml(response.Content), this);
        }

        public IEnumerable<BisqueAnalysis> GetAnalysisModuels()
        {
            var response = ModuleServiceHelper.GetAllModules(this.client);
            
            return BisqueXmlHelper.AnalysisModulesFromXmlString(response.Content);
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
            RestSharpHelpers.CheckResponse(response);
            var content = response.Content; // raw content as string
        }

        /// <summary>
        /// Downloads the specified file to a given folder.
        /// </summary>
        /// <param name="file">The file on the bisque server.</param>
        /// <param name="destinationFolder">The folder to download it to.</param>
        /// <returns> The path to the file downloaded.</returns>
        public string DownloadFile(BisqueImageResource file, string destinationFolder)
        {
            var request = new RestRequest("image_service/" + file.Id, Method.GET);
            var response = client.Execute(request);
            RestSharpHelpers.CheckResponse(response);
            var content = response.Content; // raw content as string

            byte[] fileData = response.RawBytes;
            string fileName = destinationFolder.TrimEnd('\\') + "\\" + file.ImageName;

            using (var fileStream = new FileStream(fileName, FileMode.Create, FileAccess.Write))
            {
                try
                {                 
                    fileStream.Write(fileData, 0, fileData.Length);
                    fileStream.Close();
                }
                catch (Exception e)
                {
                    Debug.WriteLine("Download failed : " + e);
                }
            }

            return fileName;
        }        

        /// <summary>
        /// Gets the metadata that is embedded in the file and could be extracted by bisque (bioformats).
        /// </summary>
        /// <param name="file">The file.</param>
        /// <returns>The Metadata as an xml string. </returns>
        public string GetEmbeddedMetaData(BisqueImageResource file)
        {
            var response = ImageServiceHelper.QueryWithParameter(this.client, file.Id, ImageServiceParameters.MetaData);                                   
            return response.Content;
        }

        public string GetFileInfo(BisqueImageResource file)
        {
            var response = ImageServiceHelper.QueryWithParameter(this.client, file.Id, ImageServiceParameters.Info);
            return response.Content;
        }

        /// <summary>
        /// Gets the bisque - metadata / tags that belonge to the file.
        /// </summary>
        /// <param name="file">The file.</param>
        /// <returns>The Metadata as an xml string. </returns>
        public string GetMetaData(BisqueImageResource file)
        {            
            var request = new RestRequest("data_service/" + file.Id +"/tag", Method.GET);            
            var response = client.Execute(request);
            RestSharpHelpers.CheckResponse(response);
            return response.Content;
        }

        /// <summary>
        /// Gets the image meta data that are allways created, like filename and upload date.
        /// </summary>
        /// <param name="file">The file.</param>
        /// <returns>The Metadata as an xml string. </returns>
        public string GetImageStandardData(BisqueImageResource file)
        {
            var request = new RestRequest("data_service/" + file.Id, Method.GET);
            var response = client.Execute(request);
            RestSharpHelpers.CheckResponse(response);            
            return response.Content;
        }                 

        /// <summary>
        /// Gets an XML document containing the current tags of an image. 
        /// This XML document can be used for tag manupulation.
        /// </summary>
        /// <param name="file">The file.</param>
        /// <returns>The xml document.</returns>
        public XmlDocument GetXmlDocumentForTagManupulation(BisqueImageResource file)
        {
            var xml = this.GetImageStandardData(file);
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(xml);

            // Add allready exitiong Tags
            AddExitingTagsToXml(file, xmlDoc);

            return xmlDoc;
        }
        
        /// <summary>
        /// Get all tags that are currently on the file and add it to the xml document.
        /// </summary>
        /// <param name="file">The file.</param>
        /// <param name="xmlDoc">The XML document.</param>
        private void AddExitingTagsToXml(BisqueImageResource file, XmlDocument xmlDoc)
        {
            var metaData = this.GetMetaData(file);
            var tags = BisqueXmlHelper.TagsFromXml(metaData);
            BisqueXmlHelper.WriteTagsToXml(xmlDoc, tags);
        }
    }  
}
