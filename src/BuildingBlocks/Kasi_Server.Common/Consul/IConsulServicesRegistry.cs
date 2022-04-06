using System.Threading.Tasks;
using Kasi_Server.Common.Consul.Models;

namespace Kasi_Server.Common.Consul;

public interface IConsulServicesRegistry
{
    Task<ServiceAgent> GetAsync(string name);
}