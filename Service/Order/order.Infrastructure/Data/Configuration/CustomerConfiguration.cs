using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using order.Domain.Models;
using order.Domain.ValueObjects;

namespace order.Infrastructure.Data.Configuration
{
    public class CustomerConfiguration : IEntityTypeConfiguration<Customer>
    {
        public void Configure(EntityTypeBuilder<Customer> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).HasConversion(id => id.Value, val => CustomerId.of(val));
            builder.Property(x => x.Name).HasMaxLength(50).IsRequired();
            builder.Property(x => x.Email).HasMaxLength(50).IsRequired();
            builder.HasIndex(x => x.Email).IsUnique();
        }
    }
}