using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using order.Domain.Models;
using order.Domain.ValueObjects;

namespace order.Infrastructure.Data.Configuration
{
    public class ProductConfiguration : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).HasConversion(id => id.Value , val => ProductId.of(val));
            builder.Property(x => x.ProductName).HasMaxLength(100).IsRequired();
            builder.Property(x => x.Price).IsRequired();
        }
    }
}