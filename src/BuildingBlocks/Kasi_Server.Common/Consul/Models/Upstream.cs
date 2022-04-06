namespace Kasi_Server.Common.Consul.Models;

public class Upstream
{
    public string DestinationName { get; set; }
    public int LocalBindPort { get; set; }
}