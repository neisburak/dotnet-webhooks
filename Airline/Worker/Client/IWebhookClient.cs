using Worker.Models;

namespace Worker.Client;

public interface IWebhookClient
{
    Task SendAsync(ChangePayload payload);
}