using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace BisqueWebHelper
{
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

        public void SetTagsFromDocument(XmlDocument xml)
        {
            this.usedSession.SetMetaData(this.resource, BisqueXmlHelper.XmlDocToString(xml));            
        }

        public void Download(string v)
        {
            this.usedSession.DownloadFile(this.resource, "c:\\tmp\\");
        }
    }
}
