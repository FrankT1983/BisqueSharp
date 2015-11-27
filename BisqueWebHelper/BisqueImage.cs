
namespace BisqueWebHelper
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using System.Xml;

    [DebuggerDisplay("{this.ImageName} {this.URI}")]
    public class BisqueImage
    {
        private readonly BisqueImageResource resource;
        private readonly BisqueSession usedSession;

        public BisqueImage(BisqueImageResource imageResource, BisqueSession session)
        {
            this.resource = imageResource;
            this.usedSession = session;
        }

        
        public string MetaData
        {
            get
            {
                return this.usedSession.GetMetaData(this.resource);
            }
        }

        public string Info
        {
            get
            {
                return this.usedSession.GetFileInfo(this.resource);
            }
        }

        public string ImageName
        {
            get
            {
                return this.resource.ImageName;
            }
        }

        public string URI
        {
            get
            {
                return this.resource.URI;
            }
        }

        internal static IEnumerable<BisqueImage> ImagesFromResources(IEnumerable<BisqueImageResource> enumerable , BisqueSession usedSession )
        {
            foreach(var res in enumerable)
            {
                yield return new BisqueImage(res, usedSession);
            }
        }

        public XmlDocument GetXmlTagDocument()
        {
            return this.usedSession.GetXmlDocumentForTagManupulation(this.resource);
        }

        /// <summary>
        /// Sets the tags of this image from a given xml document.
        /// </summary>
        /// <param name="xml">The XML document.</param>
        public void SetTagsFromDocument(XmlDocument xml)
        {
            DataServiceHelper.SetMetaData(this.usedSession.Client, this.resource, BisqueXmlHelper.XmlDocToString(xml));            
        }

        public string Download(string v)
        {
            return this.usedSession.DownloadFile(this.resource, "c:\\tmp\\");
        }

        /// <summary>
        /// Gets a specific slice of the image.
        /// </summary>
        /// <returns> The slice in bigtiff format in a byte[]</returns>
        public byte[] GetSlice(int x, int y, int z, int t)
        {
            var request = ImageServiceHelper.QueryWithParameter(this.usedSession.Client, this.resource.Id, ImageServiceParameters.Slice, ImageServiceParameters.SliceParameter(x,y,z,t));
            return request.RawBytes;
        }
    }
}
