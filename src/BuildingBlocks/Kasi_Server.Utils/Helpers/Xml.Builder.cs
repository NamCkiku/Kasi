using Kasi_Server.Utils.Extensions;
using System.Data;
using System.Xml;

namespace Kasi_Server.Utils.Helpers
{
    public partial class Xml
    {
        public XmlDocument Document { get; }

        public XmlElement Root { get; }

        public Xml(string xml = null)
        {
            Document = new XmlDocument();
            Document.LoadXml(GetXml(xml));
            Root = Document.DocumentElement;
            if (Root == null)
                throw new ArgumentException(nameof(xml));
        }

        public Xml(Stream stream)
        {
            Document = new XmlDocument();
            Document.Load(stream);
            Root = Document.DocumentElement;
            if (Root == null)
                throw new ArgumentException(nameof(stream));
        }

        private string GetXml(string xml) => string.IsNullOrWhiteSpace(xml) ? "<xml></xml>" : xml;

        public XmlNode AddNode(string name, object value = null, XmlNode parent = null)
        {
            var node = CreateNode(name, value, XmlNodeType.Element);
            GetParent(parent).AppendChild(node);
            return node;
        }

        private XmlNode GetParent(XmlNode parent) => parent ?? Root;

        private XmlNode CreateNode(string name, object value, XmlNodeType type)
        {
            var node = Document.CreateNode(type, name, string.Empty);
            if (string.IsNullOrWhiteSpace(value.SafeString()) == false)
                node.InnerText = value.SafeString();
            return node;
        }

        public XmlNode AddCDataNode(object value, XmlNode parent = null)
        {
            var node = CreateNode(Id.Guid, value, XmlNodeType.CDATA);
            GetParent(parent).AppendChild(node);
            return node;
        }

        public XmlNode AddCDataNode(object value, string parentName)
        {
            var parent = CreateNode(parentName, null, XmlNodeType.Element);
            Root.AppendChild(parent);
            return AddCDataNode(value, parent);
        }

        public void UpdateNode(string xmlPathNode, string content)
        {
            var node = Root.SelectSingleNode(xmlPathNode);
            if (node == null)
                return;
            node.InnerText = content;
        }

        public void DeleteNode(string node)
        {
            var mainNode = node.Substring(0, node.LastIndexOf("/", StringComparison.Ordinal));
            var currentNode = Root.SelectSingleNode(node);
            if (currentNode == null)
                return;
            Root.SelectSingleNode(mainNode)?.RemoveChild(currentNode);
        }

        public XmlNodeList SelectNodes(string xpath) => Root.SelectNodes(xpath);

        public DataView GetDataView(string xmlPathNode)
        {
            using (var ds = new DataSet())
            {
                var node = Root.SelectSingleNode(xmlPathNode);
                if (node == null)
                    return null;
                using (var reader = new StringReader(node.OuterXml))
                {
                    ds.ReadXml(reader);
                    return ds.Tables.Count <= 0 ? null : ds.Tables[0].DefaultView;
                }
            }
        }

        public DataView GetAllDataView(string xmlPathNode)
        {
            using (var ds = new DataSet())
            {
                using (var nodes = Root.SelectNodes(xmlPathNode))
                {
                    if (nodes == null)
                        return null;
                    foreach (XmlNode node in nodes)
                    {
                        using (var reader = new StringReader(node.OuterXml))
                            ds.ReadXml(reader);
                    }
                    return ds.Tables.Count <= 0 ? null : ds.Tables[0].DefaultView;
                }
            }
        }

        public override string ToString() => Document.OuterXml;
    }
}