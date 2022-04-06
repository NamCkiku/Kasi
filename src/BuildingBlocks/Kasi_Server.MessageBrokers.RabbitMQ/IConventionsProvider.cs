using System;

namespace Kasi_Server.MessageBrokers.RabbitMQ;

public interface IConventionsProvider
{
    IConventions Get<T>();
    IConventions Get(Type type);
}