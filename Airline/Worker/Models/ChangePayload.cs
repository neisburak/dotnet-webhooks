namespace Worker.Models;

public class ChangePayload
{
    public string WebhookUri { get; set; } = default!;
    public WebhookType WebhookType { get; set; }
    public string Publisher { get; set; } = default!;
    public Guid Secret { get; set; } = default!;
    public string FlightCode { get; set; } = default!;
    public decimal NewPrice { get; set; } = default!;
}