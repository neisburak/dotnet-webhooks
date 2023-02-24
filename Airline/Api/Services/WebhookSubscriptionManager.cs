using Api.Data;
using Api.Models;
using Api.Models.Dtos;
using Mapster;
using Microsoft.EntityFrameworkCore;

namespace Api.Services;

public class WebhookSubscriptionManager : IWebhookSubscriptionService
{
    private readonly ILogger<FlightManager> _logger;
    private readonly AirlineDbContext _context;

    public WebhookSubscriptionManager(ILogger<FlightManager> logger, AirlineDbContext context)
    {
        _logger = logger;
        _context = context;
    }

    public async Task<IEnumerable<WebhookSubscriptionForView>> GetAsync(CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();

        return await _context.WebhookSubscriptions.ProjectToType<WebhookSubscriptionForView>().ToListAsync(cancellationToken);
    }

    public async Task<WebhookSubscriptionForView?> GetAsync(int id, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();

        var subscription = await _context.WebhookSubscriptions.FirstOrDefaultAsync(f => f.Id == id, cancellationToken);

        return subscription?.Adapt<WebhookSubscriptionForView>();
    }

    public async Task<WebhookSubscriptionForView?> GetByPublisherAsync(string publisher, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();

        var subscription = await _context.WebhookSubscriptions.FirstOrDefaultAsync(f => f.WebhookPublisher == publisher, cancellationToken);

        return subscription?.Adapt<WebhookSubscriptionForView>();
    }

    public async Task<WebhookSubscriptionForView> AddAsync(WebhookSubscriptionForUpsert subscriptionForInsert, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();

        if (await _context.WebhookSubscriptions.AnyAsync(a => a.WebhookUri == subscriptionForInsert.WebhookUri, cancellationToken))
        {
            throw new Exception($"Subscription with code '{subscriptionForInsert.WebhookUri}' already exist.");
        }

        var subscription = subscriptionForInsert.Adapt<WebhookSubscription>();

        subscription.Secret = Guid.NewGuid();
        subscription.WebhookPublisher = "Airline Company";

        await _context.AddAsync(subscription, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);

        return subscription.Adapt<WebhookSubscriptionForView>();
    }

    public async Task<bool> UpdateAsync(int id, WebhookSubscriptionForUpsert subscriptionForUpdate, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();

        var subscription = await _context.WebhookSubscriptions.FirstOrDefaultAsync(a => a.Id == id, cancellationToken);

        if (subscription is null) throw new Exception($"Subscription with id '{id}' doesn't found.");

        subscription.WebhookUri = subscriptionForUpdate.WebhookUri;
        subscription.WebhookType = subscription.WebhookType;

        return await _context.SaveChangesAsync() > 0;
    }
}