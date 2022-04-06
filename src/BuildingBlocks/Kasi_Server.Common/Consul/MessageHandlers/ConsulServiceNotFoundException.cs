using System;

namespace Kasi_Server.Common.Consul.MessageHandlers;

internal sealed class ConsulServiceNotFoundException : Exception
{
    public string ServiceName { get; set; }
        
    public ConsulServiceNotFoundException(string serviceName) : this(string.Empty, serviceName)
    {
    }

    public ConsulServiceNotFoundException(string message, string serviceName) : base(message)
    {
        ServiceName = serviceName;
    }
}