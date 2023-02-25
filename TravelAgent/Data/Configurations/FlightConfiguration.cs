using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TravelAgent.Models;

namespace TravelAgent.Data.Configurations;

public class FlightConfiguration : IEntityTypeConfiguration<Flight>
{
    public void Configure(EntityTypeBuilder<Flight> builder)
    {
        builder.Property(p => p.Id).IsRequired();
        builder.Property(p => p.Code).IsRequired();
        builder.Property(p => p.Price).IsRequired().HasPrecision(18, 2);
    }
}