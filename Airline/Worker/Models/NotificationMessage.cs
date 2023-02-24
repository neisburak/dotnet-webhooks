namespace Worker.Models;

public class NotificationMessage
{
    public WebhookType WebhookType { get; set; }
    public string FlightCode { get; set; } = default!;
    public decimal NewPrice { get; set; } = default!;
}