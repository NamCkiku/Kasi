using System.Collections.Generic;

namespace Kasi_Server.MessageBrokers.RabbitMQ.Plugins;

internal interface IRabbitMqPluginsRegistryAccessor
{
    LinkedList<RabbitMqPluginChain> Get();
}