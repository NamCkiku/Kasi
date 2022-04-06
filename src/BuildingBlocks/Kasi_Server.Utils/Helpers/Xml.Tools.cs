using System.Xml;
using System.Xml.Linq;
using System.Xml.Schema;

namespace Kasi_Server.Utils.Helpers
{
    public partial class Xml
    {
        public static XDocument ToDocument(string xml) => XDocument.Parse(xml);

        public static List<XElement> ToElements(string xml)
        {
            var document = ToDocument(xml);
            return document?.Root == null ? new List<XElement>() : document.Root.Elements().ToList();
        }

        public static void Validate(string xmlFile, string schemaFile)
        {
            if (string.IsNullOrWhiteSpace(xmlFile))
                throw new ArgumentNullException(nameof(xmlFile));
            if (string.IsNullOrWhiteSpace(schemaFile))
                throw new ArgumentNullException(nameof(schemaFile));
            XmlReader reader = null;
            try
            {
                var result = new Tuple<bool, string>(true, string.Empty);
                var settings = new XmlReaderSettings();
                settings.ValidationType = ValidationType.Schema;
                settings.ValidationFlags |= XmlSchemaValidationFlags.ProcessInlineSchema;
                settings.ValidationFlags |= XmlSchemaValidationFlags.ReportValidationWarnings;
                settings.Schemas.Add(null, schemaFile);
                settings.ValidationEventHandler += (obj, e) =>
                {
                    result = new Tuple<bool, string>(false, e.Message);
                };
                using (reader = XmlReader.Create(xmlFile, settings))
                {
                    while (reader.Read()) { }
                }
                if (!result.Item1)
                    throw new ArgumentException(result.Item2);
            }
            finally
            {
                reader?.Close();
            }
        }
    }
}