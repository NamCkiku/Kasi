using Kasi_Server.Utils.Extensions;
using System.Runtime.InteropServices;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Xml.Serialization;

namespace Kasi_Server.Utils.Helpers
{
    public static class Serialize
    {
        public static byte[] ToBytes(object data)
        {
            var bytes = new byte[Marshal.SizeOf(data)];
            var ptr = Marshal.UnsafeAddrOfPinnedArrayElement(bytes, 0);
            Marshal.StructureToPtr(data, ptr, true);
            return bytes;
        }

        public static T FromBytes<T>(byte[] bytes)
        {
            var type = typeof(T);
            var ptr = Marshal.UnsafeAddrOfPinnedArrayElement(bytes, 0);
            var obj = Marshal.PtrToStructure(ptr, type);
            return (T)obj;
        }

        public static byte[] ToBinary(object data)
        {
            data.CheckNotNull(nameof(data));
            using (var ms = new MemoryStream())
            {
                var formatter = new BinaryFormatter();
                formatter.Serialize(ms, data);
                ms.Seek(0, SeekOrigin.Begin);
                return ms.ToArray();
            }
        }

        public static T FromBinary<T>(byte[] bytes)
        {
            bytes.CheckNotNullOrEmpty(nameof(bytes));
            using (var ms = new MemoryStream())
            {
                var formatter = new BinaryFormatter();
                return (T)formatter.Deserialize(ms);
            }
        }

        public static void ToBinaryFile(string fileName, object data)
        {
            fileName.CheckNotNull(nameof(fileName));
            data.CheckNotNull(nameof(data));
            using (var fs = new FileStream(fileName, FileMode.Create))
            {
                var formatter = new BinaryFormatter();
                formatter.Serialize(fs, data);
            }
        }

        public static T FromBinaryFile<T>(string fileName)
        {
            fileName.CheckFileExists(nameof(fileName));
            using (var fs = new FileStream(fileName, FileMode.Open))
            {
                var formatter = new BinaryFormatter();
                return (T)formatter.Deserialize(fs);
            }
        }

        public static string ToXml(object data)
        {
            data.CheckNotNull(nameof(data));
            using (var ms = new MemoryStream())
            {
                var serializer = new XmlSerializer(data.GetType());
                serializer.Serialize(ms, data);
                ms.Seek(0, SeekOrigin.Begin);
                return Encoding.Default.GetString(ms.ToArray());
            }
        }

        public static T FromXml<T>(string xml)
        {
            xml.CheckNotNull(nameof(xml));
            byte[] bytes = Encoding.Default.GetBytes(xml);
            using (var ms = new MemoryStream(bytes))
            {
                var serializer = new XmlSerializer(typeof(T));
                return (T)serializer.Deserialize(ms);
            }
        }

        public static void ToXmlFile(string fileName, object data)
        {
            fileName.CheckNotNull(nameof(fileName));
            data.CheckNotNull(nameof(data));
            using (var fs = new FileStream(fileName, FileMode.Create))
            {
                var serializer = new XmlSerializer(data.GetType());
                serializer.Serialize(fs, data);
            }
        }

        public static T FromXmlFile<T>(string fileName)
        {
            fileName.CheckFileExists(nameof(fileName));
            using (var fs = new FileStream(fileName, FileMode.Open))
            {
                var serializer = new XmlSerializer(typeof(T));
                return (T)serializer.Deserialize(fs);
            }
        }
    }
}