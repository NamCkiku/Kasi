using System.Threading.Tasks;
using RabbitMQ.Client.Events;

namespace Kasi_Server.MessageBrokers.RabbitMQ;

public interface IRabbitMqPlugin
{
    Task HandleAsync(object message, object correlationContext, BasicDeliverEventArgs args);
}