using System.Threading.Tasks;

namespace Kasi_Server.Common
{
    public interface IInitializer
    {
        Task InitializeAsync();
    }
}