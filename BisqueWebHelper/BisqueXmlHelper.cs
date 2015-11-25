namespace BisqueWebHelper
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using System.Xml;

    public static class BisqueXmlHelper
    {
        /// <summary>
        /// Extract the imageses from a given XML reply from the bisuqe server.
        /// </summary>
        /// <param name="xml">The XML string.</param>
        /// <returns>The images.</returns>
        internal static IEnumerable<BisqueImageResource> ImagesFromXml(string xml)
        {
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(xml);

            string xpath = "resource/image";
            var nodes = xmlDoc.SelectNodes(xpath);

            foreach (XmlNode node in nodes)
            {
                var name = node.Attributes["name"];
                var uri = node.Attributes["uri"];
                var id = node.Attributes["resource_uniq"];


                if (name == null || uri == null || id == null)
                {
                    continue;
                }

                var image = new BisqueImageResource
                {
                    ImageName = name.Value,
                    URI = uri.Value,
                    Id = id.Value,
                };

                yield return image;
            }
        }

        /// <summary>
        /// Extract the tages of a image from a given XML reply from the bisuqe server.
        /// </summary>
        /// <param name="xml">The XML.</param>
        /// <returns>The tags.</returns>
        public static IEnumerable<Tuple<string, string>> TagsFromXml(string xml)
        {
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(xml);

            string xpath = "resource/tag";
            var nodes = xmlDoc.SelectNodes(xpath);

            foreach (XmlNode node in nodes)
            {
                var name = node.Attributes["name"];
                var value = node.Attributes["value"];

                if (name == null || value == null)
                {
                    continue;
                }

                yield return new Tuple<string, string>(name.Value, value.Value);
            }
        }

        public static void WriteTagsToXml(XmlDocument xmlDoc, IEnumerable<Tuple<string, string>> tags)
        {
            XmlNode tagsNode = xmlDoc.SelectSingleNode("image");

            foreach (var tag in tags)
            {
                XmlNode newTag = xmlDoc.CreateNode(XmlNodeType.Element, "tag", null);
                var nameAttribute = xmlDoc.CreateAttribute("name");
                var valueAttribute = xmlDoc.CreateAttribute("value");

                nameAttribute.Value = tag.Item1;
                valueAttribute.Value = tag.Item2;

                newTag.Attributes.Append(nameAttribute);
                newTag.Attributes.Append(valueAttribute);

                tagsNode.AppendChild(newTag);
            }
        }

        public static void WriteTagsToXml(XmlDocument xmlDoc, Tuple<string, string> tag)
        {
            var tempList = new List<Tuple<string, string>>();
            tempList.Add(tag);
            WriteTagsToXml(xmlDoc, tempList);
        }

        public static string XmlDocToString(XmlDocument xmlDoc)
        {
            var settings = new XmlWriterSettings();
            settings.OmitXmlDeclaration = true;
            using (var stringWriter = new StringWriter())
            using (var xmlTextWriter = XmlWriter.Create(stringWriter, settings))
            {
                xmlDoc.WriteTo(xmlTextWriter);
                xmlTextWriter.Flush();

                return stringWriter.GetStringBuilder().ToString();
            }
        }
    }
}
