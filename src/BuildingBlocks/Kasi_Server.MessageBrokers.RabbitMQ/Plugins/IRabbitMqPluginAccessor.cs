using System;
using System.Threading.Tasks;
using RabbitMQ.Client.Events;

namespace Kasi_Server.MessageBrokers.RabbitMQ.Plugins;

internal interface IRabbitMqPluginAccessor
{
    void SetSuccessor(Func<object, object, BasicDeliverEventArgs, Task> successor);
}