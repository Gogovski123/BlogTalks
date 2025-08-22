namespace BlogTalks.EmailSenderAPI.Models
{
    public class RabbitMqSettingsEmailSender
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
