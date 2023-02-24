using Api.Models.Dtos;

namespace Api.Services;

public interface IWebhookSubscriptionService
{
    Task<IEnumerable<WebhookSubscriptionForView>> GetAsync(CancellationToken cancellationToken = default);
    Task<WebhookSubscriptionForView?> GetAsync(int id, CancellationToken cancellationToken = default);
    Task<WebhookSubscriptionForView?> GetByPublisherAsync(string publisher, CancellationToken cancellationToken = default);
    Task<WebhookSubscriptionForView> AddAsync(WebhookSubscriptionForUpsert flightForInsert, CancellationToken cancellationToken = default);
    Task<bool> UpdateAsync(int id, WebhookSubscriptionForUpsert flightForUpdate, CancellationToken cancellationToken = default);
}