using System.Xml;
using System.Xml.Linq;

namespace Kasi_Server.Utils.Extensions
{
    public static class XmlExtensions
    {
        public static XElement ToXElement(this XmlNode node)
        {
            XDocument xdoc = new XDocument();
            using (var xmlWriter = xdoc.CreateWriter())
            {
                node.WriteTo(xmlWriter);
            }

            return xdoc.Root;
        }

        public static XmlNode ToXmlNode(this XElement element)
        {
            using (var xmlReader = element.CreateReader())
            {
                var xml = new XmlDocument();
                xml.Load(xmlReader);
                return xml;
            }
        }
    }
}