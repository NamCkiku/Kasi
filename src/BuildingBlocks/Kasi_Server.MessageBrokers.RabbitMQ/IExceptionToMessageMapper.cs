using System;

namespace Kasi_Server.MessageBrokers.RabbitMQ;

public interface IExceptionToMessageMapper
{
    object Map(Exception exception, object message);
}