using System.Collections.Generic;

namespace Kasi_Server.MessageBrokers.RabbitMQ;

public interface IContextProvider
{
    string HeaderName { get; }
    object Get(IDictionary<string, object> headers);
}