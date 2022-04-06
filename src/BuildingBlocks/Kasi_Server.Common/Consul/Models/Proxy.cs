using System.Collections.Generic;

namespace Kasi_Server.Common.Consul.Models;

public class Proxy
{
    public List<Upstream> Upstreams { get; set; }
}