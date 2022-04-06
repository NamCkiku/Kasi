using System.IO;
using System.Threading.Tasks;

namespace Kasi_Server.Common.HTTP;

public interface IHttpClientSerializer
{
    string Serialize<T>(T value);
    ValueTask<T> DeserializeAsync<T>(Stream stream);
}