using BlogTalks.Application.Abstractions;
using BlogTalks.Application.Contracts;
using BlogTalks.EmailSenderAPI.Models;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using RabbitMQ.Client;
using System.Text;

namespace BlogTalks.Infrastructure.Messaging
{
    public class MessagingRabbitMqService : IMessagingService
    {
        private readonly RabbitMqSettings _settings;

        public MessagingRabbitMqService(IOptions<RabbitMqSettings> settings)
        {
            _settings = settings.Value;
        }

        public async Task Send(Application.Contracts.EmailDto email)
        {
            var exchangeName = _settings.ExchangeName;
            var exchangeType = _settings.ExchangeType;
            var queueName = _settings.QueueName;
            var routeKey = _settings.RouteKey;

            var factory = new ConnectionFactory
            {
                HostName = _settings.RabbitURL,
                UserName = _settings.Username,
                Password = _settings.Password
            };

            await using var connection = await factory.CreateConnectionAsync();
            await using var channel = await connection.CreateChannelAsync();

            await channel.ExchangeDeclareAsync(
                exchange: exchangeName,
                type: exchangeType,
                durable: true
            );

            await channel.QueueDeclareAsync(
                queue: queueName,
                durable: true,
                exclusive: false,
                autoDelete: false
            );

            await channel.QueueBindAsync(
                queue: queueName,
                exchange: exchangeName,
                routingKey: routeKey
            );

            var body = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(email));

            var properties = new BasicProperties
            {
                Persistent = true
            };

            await channel.BasicPublishAsync(
                exchange: exchangeName,
                routingKey: routeKey,
                mandatory: false,
                basicProperties: properties,
                body: body
            );
        }
    }
}
    

