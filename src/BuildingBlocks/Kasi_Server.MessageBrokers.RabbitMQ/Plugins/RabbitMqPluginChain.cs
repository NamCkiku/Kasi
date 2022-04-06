using System;

namespace Kasi_Server.MessageBrokers.RabbitMQ.Plugins;

internal sealed class RabbitMqPluginChain
{
    public Type PluginType { get; set; }
}