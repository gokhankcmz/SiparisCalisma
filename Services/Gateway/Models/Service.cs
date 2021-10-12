namespace Gateway.Models
{
    public class Service
    {
        public string ServiceName { get; set; }
        public string HostName { get; set; }
        public string Port { get; set; }

        public string[] RoutingKeys { get; set; }

    }
}