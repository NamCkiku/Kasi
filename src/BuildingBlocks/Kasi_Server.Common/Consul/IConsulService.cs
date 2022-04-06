using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Kasi_Server.Common.Consul.Models;

namespace Kasi_Server.Common.Consul;

public interface IConsulService
{
    Task<HttpResponseMessage> RegisterServiceAsync(ServiceRegistration registration);
    Task<HttpResponseMessage> DeregisterServiceAsync(string id);
    Task<IDictionary<string, ServiceAgent>> GetServiceAgentsAsync(string service = null);
}