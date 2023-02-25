using System.Text;
using System.Text.Json;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using Worker.Client;
using Worker.Data;
using Worker.Models;

namespace Worker.App;

public class AppHost : IAppHost
{
    private readonly IWebhookClient _client;
    private readonly AirlineDbContext _context;
    private readonly ILogger<AppHost> _logger;

    public AppHost(ILogger<AppHost> logger, AirlineDbContext context, IWebhookClient client)
    {
        _logger = logger;
        _context = context;
        _client = client;
    }

    public void Run()
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

        var queueName = channel.QueueDeclare().QueueName;

        channel.QueueBind(queue: queueName, exchange: "trigger", routingKey: string.Empty);

        var consumer = new EventingBasicConsumer(channel);

        _logger.LogInformation("Listening on the message bus...");

        consumer.Received += async (sender, args) =>
        {
            _logger.LogInformation("Event is triggered!");

            var message = GetMessage(args.Body.ToArray());
            if (message is null) return;

            foreach (var whs in _context.WebhookSubscriptions.Where(w => w.WebhookType == message.WebhookType))
            {
                var webhookToSend = new ChangePayload
                {
                    NewPrice = message.NewPrice,
                    FlightCode = message.FlightCode,
                };

                await _client.SendAsync(whs.WebhookUri, webhookToSend, new Dictionary<string, string>
                {
                    { "Secret", whs.Secret.ToString() },
                    { "Publisher", whs.WebhookPublisher },
                    { "Event-Type", message.WebhookType.ToString() },
                });
            }
        };

        channel.BasicConsume(queue: queueName, autoAck: true, consumer: consumer);

        Console.Read();
    }

    private NotificationMessage? GetMessage(byte[] bytes)
    {
        var payload = Encoding.UTF8.GetString(bytes);
        return JsonSerializer.Deserialize<NotificationMessage>(payload);
    }
}