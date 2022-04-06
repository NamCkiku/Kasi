using System.Text.Json.Serialization;

namespace Kasi_Server.Common.Consul.Models;

public class Connect
{
    [JsonPropertyName("sidecar_service")]
    public SidecarService SidecarService { get; set; }
}