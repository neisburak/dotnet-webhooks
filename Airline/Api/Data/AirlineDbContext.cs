using Api.Models;
using Microsoft.EntityFrameworkCore;

namespace Api.Data;

public class AirlineDbContext : DbContext
{
    public AirlineDbContext(DbContextOptions options) : base(options) { }

    public DbSet<Flight> Flights => Set<Flight>();
    public DbSet<WebhookSubscription> WebhookSubscriptions => Set<WebhookSubscription>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AirlineDbContext).Assembly);
    }
}