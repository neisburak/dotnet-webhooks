using Api.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Api.Data.Configurations;

public class WebhookSubscriptionConfiguration : IEntityTypeConfiguration<WebhookSubscription>
{
    public void Configure(EntityTypeBuilder<WebhookSubscription> builder)
    {
        builder.Property(p => p.Id).IsRequired();
        builder.Property(p => p.WebhookUri).IsRequired();
        builder.Property(p => p.Secret).IsRequired();
        builder.Property(p => p.WebhookType).IsRequired();
        builder.Property(p => p.WebhookPublisher).IsRequired();
    }
}