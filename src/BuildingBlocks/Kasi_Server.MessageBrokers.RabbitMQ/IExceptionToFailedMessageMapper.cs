using System;

namespace Kasi_Server.MessageBrokers.RabbitMQ;

public interface IExceptionToFailedMessageMapper
{
    FailedMessage Map(Exception exception, object message);
}