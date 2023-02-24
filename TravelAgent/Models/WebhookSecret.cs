namespace TravelAgent.Models;

public class WebhookSecret
{
    public int Id { get; set; }
    public Guid Secret { get; set; } = default!;
    public string Publisher { get; set; } = default!;
}