using Microsoft.EntityFrameworkCore;
using TravelAgent.Models;

namespace TravelAgent.Data;

public class TravelAgentDbContext : DbContext
{
    public TravelAgentDbContext(DbContextOptions<TravelAgentDbContext> options) : base(options) { }

    public DbSet<WebhookSecret> SubscriptionSecrets => Set<WebhookSecret>();
}
