using Worker.Models;

namespace Worker.Client;

public interface IWebhookClient
{
    Task SendAsync(string uri, ChangePayload payload, IDictionary<string, string> headers);
}