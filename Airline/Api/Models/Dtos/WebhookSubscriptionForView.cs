namespace Api.Models.Dtos;

public class WebhookSubscriptionForView
{
    public string WebhookUri { get; set; } = default!;
    public string Secret { get; set; } = default!;
    public string WebhookType { get; set; } = default!;
    public string WebhookPublisher { get; set; } = default!;
}