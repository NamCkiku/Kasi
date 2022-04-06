using System;
using System.Collections.Generic;
using System.Text;

namespace Kasi_Server.Logging
{
    public class RabbitMQSinksOptions
    {
        public bool Enabled { get; set; }
        public string Hostname { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }

        public string Exchange { get; set; }

        public string ExchangeType { get; set; }

        public string RouteKey { get; set; }
        public int Port { get; set; }
        public string ApplicationName { get; set; }

    }
}
