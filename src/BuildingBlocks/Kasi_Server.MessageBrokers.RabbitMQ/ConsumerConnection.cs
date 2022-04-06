﻿using RabbitMQ.Client;

namespace Kasi_Server.MessageBrokers.RabbitMQ;

public sealed class ConsumerConnection
{
    public IConnection Connection { get; }

    public ConsumerConnection(IConnection connection)
    {
        Connection = connection;
    }
}