using Kasi_Server.Utils.Helpers;
using System.Xml;

namespace Kasi_Server.Utils.Extensions
{
    public static class XmlNodeExtensions
    {
        public static XmlNode CreateChildNode(this XmlNode parentNode, string name, string namespaceUri = "")
        {
            var document = parentNode is XmlDocument xmlDocument ? xmlDocument : parentNode.OwnerDocument;
            if (document == null)
            {
                throw new ArgumentException(nameof(document));
            }

            var node = !string.IsNullOrWhiteSpace(namespaceUri)
                ? document.CreateElement(name, namespaceUri)
                : document.CreateElement(name);
            parentNode.AppendChild(node);
            return node;
        }

        public static XmlCDataSection CreateCDataSection(this XmlNode parentNode, string data = "")
        {
            var document = parentNode is XmlDocument xmlDocument ? xmlDocument : parentNode.OwnerDocument;
            if (document == null)
            {
                throw new ArgumentException(nameof(document));
            }

            var node = document.CreateCDataSection(data);
            parentNode.AppendChild(node);
            return node;
        }

        public static string GetCdataSection(this XmlNode parentNode)
        {
            return parentNode.ChildNodes.OfType<XmlCDataSection>().Select(node => node.Value).FirstOrDefault();
        }

        public static string GetAttribute(this XmlNode node, string name, string defaultValue = null)
        {
            if (node.Attributes == null)
            {
                return defaultValue;
            }
            var attribute = node.Attributes[name];
            return attribute != null ? attribute.InnerText : defaultValue;
        }

        public static T GetAttribute<T>(this XmlNode node, string name, T defaultValue = default(T))
        {
            var value = node.GetAttribute(name);
            if (string.IsNullOrWhiteSpace(value))
            {
                return defaultValue;
            }

            return Conv.To<T>(value);
        }

        public static void SetAttribute(this XmlNode node, string name, object value)
        {
            SetAttribute(node, name, value.SafeString());
        }

        public static void SetAttribute(this XmlNode node, string name, string value)
        {
            if (node == null)
            {
                return;
            }

            if (string.IsNullOrWhiteSpace(name) || string.IsNullOrWhiteSpace(value))
            {
                return;
            }

            if (node.Attributes == null)
            {
                return;
            }
            var attribute = node.Attributes[name, node.NamespaceURI];
            if (attribute == null)
            {
                attribute = node.OwnerDocument?.CreateAttribute(name, node.OwnerDocument.NamespaceURI);
                node.Attributes.Append(attribute);
            }

            attribute.InnerText = value;
        }
    }
}