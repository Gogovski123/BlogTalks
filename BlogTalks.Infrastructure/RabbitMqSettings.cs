using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogTalks.Infrastructure
{
    public class RabbitMqSettings
    {
        public string ExchangeName { get; set; }
        public string ExchangeType { get; set; }
        public string QueueName { get; set; }
        public string RouteKey { get; set; }
        public string RabbitURL { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
    }
}
