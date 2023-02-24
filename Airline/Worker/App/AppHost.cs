using System.Text;
using System.Text.Json;
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

            var webhookToSend = new ChangePayload
            {
                WebhookType = message.WebhookType,
                NewPrice = message.NewPrice,
                FlightCode = message.FlightCode,
            };

            foreach (var whs in _context.WebhookSubscriptions.Where(w => w.WebhookType == webhookToSend.WebhookType))
            {
                webhookToSend.WebhookUri = whs.WebhookUri;
                webhookToSend.Secret = whs.Secret;
                webhookToSend.Publisher = whs.WebhookPublisher;

                await _client.SendAsync(webhookToSend);
            }
        };

        channel.BasicConsume(queue: queueName, autoAck: true, consumer: consumer);

        Console.ReadKey();
    }

    private NotificationMessage? GetMessage(byte[] bytes)
    {
        var payload = Encoding.UTF8.GetString(bytes);
        return JsonSerializer.Deserialize<NotificationMessage>(payload);
    }
}