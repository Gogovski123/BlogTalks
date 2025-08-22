using BlogTalks.EmailSenderAPI.Models;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

namespace BlogTalks.EmailSenderAPI.Services;

public class RabbitMqBackgroundEmailService : BackgroundService
{
    private IConnection _connection;
    private IChannel _channel;
    private readonly IServiceScopeFactory _serviceScopeFactory;
    private readonly RabbitMqSettingsEmailSender _rabbitMqSettings;

    public RabbitMqBackgroundEmailService(IOptions<RabbitMqSettingsEmailSender> rabbitmqSettings, IServiceScopeFactory serviceScopeFactory)
    {
        _serviceScopeFactory = serviceScopeFactory;
        _rabbitMqSettings = rabbitmqSettings.Value;
    }

    private async Task InitRabbitMQ()
    {
        var factory = new ConnectionFactory
        {
            HostName = _rabbitMqSettings.RabbitURL,
            UserName = _rabbitMqSettings.Username,
            Password = _rabbitMqSettings.Password,
        };
        _connection = await factory.CreateConnectionAsync();
        _channel = await _connection.CreateChannelAsync();

        await _channel.ExchangeDeclareAsync(_rabbitMqSettings.ExchangeName, _rabbitMqSettings.ExchangeType, durable: true);
        await _channel.QueueDeclareAsync(_rabbitMqSettings.QueueName,
            durable: true,
            exclusive: false,
            autoDelete: false,
            arguments: null);

        await _channel.QueueBindAsync(_rabbitMqSettings.QueueName, _rabbitMqSettings.ExchangeName,
            _rabbitMqSettings.RouteKey);
        await _channel.BasicQosAsync(prefetchSize: 0, prefetchCount: 1, global: false);

        _connection.ConnectionShutdownAsync += RabbitMQ_ConnectionShutdown;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        stoppingToken.ThrowIfCancellationRequested();

        await InitRabbitMQ();

        var consumer = new AsyncEventingBasicConsumer(_channel);
        consumer.ReceivedAsync += async (a, b) =>
        {
            using var scope = _serviceScopeFactory.CreateScope();
            var emailSender = scope.ServiceProvider.GetRequiredService<IEmailSender>();
            var message = Encoding.UTF8.GetString(b.Body.ToArray());
            var emailDetails = JsonConvert.DeserializeObject<EmailDto>(message);

            await emailSender.SendEmailAsync(emailDetails);
            await _channel.BasicAckAsync(b.DeliveryTag, false, stoppingToken);
        };

        consumer.ShutdownAsync += OnConsumerShutdown;
        consumer.RegisteredAsync += OnConsumerRegistered;
        consumer.UnregisteredAsync += OnConsumerUnregistered;

        await _channel.BasicConsumeAsync(_rabbitMqSettings.QueueName, false, consumer, cancellationToken: stoppingToken);
    }
    private async Task OnConsumerUnregistered(object sender, ConsumerEventArgs e) { }
    private async Task OnConsumerRegistered(object sender, ConsumerEventArgs e) { }
    private async Task OnConsumerShutdown(object sender, ShutdownEventArgs e) { }
    private async Task RabbitMQ_ConnectionShutdown(object sender, ShutdownEventArgs e) { }

    public override void Dispose()
    {
        _channel.CloseAsync();
        _connection.CloseAsync();
        base.Dispose();
    }
}
