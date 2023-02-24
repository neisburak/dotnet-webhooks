namespace Api.Models.Dtos;

public class WebhookSubscriptionForUpsert
{
    public string WebhookUri { get; set; } = default!;
    public string WebhookType { get; set; } = default!;
}