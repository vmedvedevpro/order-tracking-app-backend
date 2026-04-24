using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using OrderTrackingApp.Backend.Domain.Entities;

namespace OrderTrackingApp.Backend.Infrastructure.Persistence.Configuration;

public class OrderConfiguration : IEntityTypeConfiguration<Order>
{
    public void Configure(EntityTypeBuilder<Order> builder)
    {
        builder.HasKey(o => o.OrderNumber);

        builder.Property(o => o.Description)
               .IsRequired();

        builder.Property(o => o.Status)
               .HasConversion<string>()
               .HasMaxLength(50)
               .IsRequired();

        builder.Property(o => o.CreatedAt)
               .IsRequired();

        builder.Property(o => o.UpdatedAt)
               .IsRequired();
    }
}
