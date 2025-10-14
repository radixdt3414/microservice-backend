using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using order.Domain.Models;
using order.Domain.ValueObjects;

namespace order.Infrastructure.Data.Configuration
{
    public class OrderItemConfiguration : IEntityTypeConfiguration<OrderItem>
    {
        public void Configure(EntityTypeBuilder<OrderItem> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).HasConversion(id => id.Value, val => OrderItemId.of(val));
            builder.Property(x => x.ProductId).HasConversion(id => id.Value, val => ProductId.of(val));
            builder.Property(x => x.OrderId).HasConversion(id => id.Value, val => OrderId.of(val));

            builder.Property(x => x.Quentity).IsRequired();
            builder.Property(x => x.Price).IsRequired();
            //relation
            builder.HasOne<Product>().WithMany().HasForeignKey(x => x.ProductId).IsRequired();
        }
    }
}