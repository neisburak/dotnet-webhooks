using System.Text;
using System.Text.Json;
using Api.Models;
using RabbitMQ.Client;

namespace Api.MessageBus;

public class RabbitMQMessageBus : IMessageBus
{
    private readonly ILogger<RabbitMQMessageBus> _logger;

    public RabbitMQMessageBus(ILogger<RabbitMQMessageBus> logger)
    {
        _logger = logger;
    }

    public void Publish(NotificationMessage notificationMessage)
    {
        var factory = new ConnectionFactory
        {
            HostName = "localhost",
            Port = 5672,
            UserName = "guest",
            Password = "guest"
        };

        using var connection = factory.CreateConnection();
        using var channel = connection.CreateModel();

        channel.ExchangeDeclare(exchange: "trigger", type: ExchangeType.Fanout);

        var message = GetMessage(notificationMessage);

        channel.BasicPublish(exchange: "trigger", routingKey: string.Empty, basicProperties: null, body: message);
        
        _logger.LogInformation("Message published on message bus.");
    }

    private byte[] GetMessage(NotificationMessage notificationMessage)
    {
        var message = JsonSerializer.Serialize(notificationMessage);
        return Encoding.UTF8.GetBytes(message);
    }
}