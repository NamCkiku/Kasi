using System.Threading.Tasks;

namespace Kasi_Server.Types;

public interface IInitializer
{
    Task InitializeAsync();
}