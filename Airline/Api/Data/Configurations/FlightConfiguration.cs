using Api.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Api.Data.Configurations;

public class FlightConfiguration : IEntityTypeConfiguration<Flight>
{
    public void Configure(EntityTypeBuilder<Flight> builder)
    {
        builder.Property(p => p.Id).IsRequired();
        builder.Property(p => p.Code).IsRequired();
        builder.Property(p => p.Price).IsRequired();
        builder.Property(p => p.Price).HasPrecision(18, 2);
    }
}