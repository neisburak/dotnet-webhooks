namespace Api.Models.Dtos;

public class WebhookSubscriptionForUpsert
{
    public string WebhookUri { get; set; } = default!;
    public WebhookType WebhookType { get; set; } = default!;
}