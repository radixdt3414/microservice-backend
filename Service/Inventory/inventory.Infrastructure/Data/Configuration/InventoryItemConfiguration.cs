using inventory.Domain.Models;
using inventory.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace inventory.Infrastructure.Data.Configuration
{
    public class InventoryItemConfiguration : IEntityTypeConfiguration<InventoryItem>
    {
        public void Configure(EntityTypeBuilder<InventoryItem> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).HasConversion(id => id.Value, val => InventoryItemId.Of(val));
            builder.Property(x => x.ProductId).HasConversion(id => id.Value, val => ProductId.Of(val));
            builder.Property(x => x.QuantityReserved).IsRequired();
            builder.Property(x => x.QuantityAvailable).IsRequired();
            builder.Property(x => x.QuantityOnHand).IsRequired();
        }
    }
}