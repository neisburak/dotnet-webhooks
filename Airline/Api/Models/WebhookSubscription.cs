namespace Api.Models;

public class WebhookSubscription
{
    public int Id { get; set; }
    public string WebhookUri { get; set; } = default!;
    public Guid Secret { get; set; } = default!;
    public WebhookType WebhookType { get; set; } = default!;
    public string WebhookPublisher { get; set; } = default!;
}