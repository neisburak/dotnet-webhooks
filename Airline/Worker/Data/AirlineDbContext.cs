using Microsoft.EntityFrameworkCore;
using Worker.Models;

namespace Worker.Data;

public class AirlineDbContext : DbContext
{
    public AirlineDbContext(DbContextOptions<AirlineDbContext> options) : base(options) { }

    public DbSet<WebhookSubscription> WebhookSubscriptions => Set<WebhookSubscription>();
}