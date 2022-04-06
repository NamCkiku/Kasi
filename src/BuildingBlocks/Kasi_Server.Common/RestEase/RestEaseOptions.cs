using System.Collections.Generic;

namespace Kasi_Server.Common.RestEase
{
    public class RestEaseOptions
    {
        public string LoadBalancer { get; set; }
        public List<Service> Services { get; set; }
    }

    public class Service
    {
        public string Name { get; set; }
        public string Scheme { get; set; }
        public string Host { get; set; }
        public int Port { get; set; }
    }
}