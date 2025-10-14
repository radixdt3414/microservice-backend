using inventory.Domain.Models;
using inventory.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace inventory.Infrastructure.Data.Configuration
{
    public class WareHouseConfiguration : IEntityTypeConfiguration<Warehouse>
    {
        public void Configure(EntityTypeBuilder<Warehouse> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).HasConversion(id => id.Value, val => WarehouseId.Of(val));
            builder.ComplexProperty(x => x.Address, address =>
            {
                address.Property(x => x.Country).IsRequired().HasColumnName("country");
                address.Property(x => x.Landmark ).IsRequired().HasColumnName("Landmark");
                address.Property(x => x.State ).IsRequired().HasColumnName("State");
                address.Property(x => x.City ).IsRequired().HasColumnName("City");
                address.Property(x => x.PostalCode ).IsRequired().HasColumnName("PostalCode");
                address.Property(x => x.Description).IsRequired().HasColumnName("Description");
            });
            builder.Property(x => x.Longitude).IsRequired(false);
            builder.Property(x => x.Latitude).IsRequired(false);
            builder.Property(x => x.CurrentCapacityUnitUsed).IsRequired();
            builder.Property(x => x.CapacityUnit).IsRequired();
            builder.HasMany(x => x.InventoryItem).WithOne().HasForeignKey(x => x.WarehouseId);
        }
    }
}