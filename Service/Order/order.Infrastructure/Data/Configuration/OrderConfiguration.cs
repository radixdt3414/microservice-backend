using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using order.Domain.Enum;
using order.Domain.Models;
using order.Domain.ValueObjects;
using System.Reflection.Emit;

namespace order.Infrastructure.Data.Configuration
{
    internal class OrderConfiguration : IEntityTypeConfiguration<Order>
    {
        public void Configure(EntityTypeBuilder<Order> builder)
        {
            builder.HasKey(x => x.Id);
            
            builder.Property(x => x.Id).HasConversion(Id => Id.Value, val => OrderId.of(val));
            
            builder.ComplexProperty(x => x.ShippingAddress, address =>
            {
                address.Property(x => x.City).IsRequired(true).HasMaxLength(50);
                address.Property(x => x.Country).IsRequired(true).HasMaxLength(50);  
                address.Property(x => x.State).IsRequired(true).HasMaxLength(50);
                address.Property(x => x.FirstName).IsRequired(true).HasMaxLength(50);
                address.Property(x => x.LastName).IsRequired(true).HasMaxLength(50);
                address.Property(x => x.Landmark).IsRequired(true).HasMaxLength(50);
                address.Property(x => x.PostalCode).IsRequired(true).HasMaxLength(50);
                address.Property(x => x.Description).HasMaxLength(250);
            });

            builder.ComplexProperty(x => x.OrderAddress, address =>
            {
                address.Property(x => x.City).IsRequired(true).HasMaxLength(50);
                address.Property(x => x.Country).IsRequired(true).HasMaxLength(50);
                address.Property(x => x.State).IsRequired(true).HasMaxLength(50);
                address.Property(x => x.FirstName).IsRequired(true).HasMaxLength(50);
                address.Property(x => x.LastName).IsRequired(true).HasMaxLength(50);
                address.Property(x => x.Landmark).IsRequired(true).HasMaxLength(50);
                address.Property(x => x.PostalCode).IsRequired(true).HasMaxLength(50);
                address.Property(x => x.Description).HasMaxLength(250);
            });

            builder.Property(x => x.Name).HasConversion(x => x.Value, val => OrderName.of(val)).IsRequired();
            
            builder.Property(x => x.CustomerId).HasConversion(customerId => customerId.Value, val => CustomerId.of(val));

            builder.Property(x => x.Status).HasConversion(status => status.ToString(), val => (OrderStatus)Enum.Parse(typeof(OrderStatus),val));

            builder.ComplexProperty(x => x.Payment, payment =>
            {
                payment.Property(x => x.CardNumber).HasMaxLength(24).IsRequired(true);
                payment.Property(x => x.Cvv).HasMaxLength(3).IsRequired(true);
                payment.Property(x => x.ExpiryDate).IsRequired(true);
                payment.Property(x => x.PaymentType).IsRequired().HasMaxLength(50);
                payment.Property(x => x.CardMemberName).HasMaxLength(100).IsRequired(true);
            });

            builder.Property(x => x.TotalPrice);

            //relation
            //builder.HasMany(x => x._OrderItems).WithOne().HasForeignKey(x => x.OrderId);
            builder.HasMany<OrderItem>(x => x.OrderItems) 
            .WithOne()                         
            .HasForeignKey(x => x.OrderId);         

            //builder.Metadata.FindNavigation(nameof(Order.OrderItems))!.SetPropertyAccessMode(PropertyAccessMode.Property);

                //.Navigation(e => e._OrderItems)
                //.UsePropertyAccessMode(PropertyAccessMode.Field);

            builder.HasOne<Customer>().WithMany().HasForeignKey(x => x.CustomerId);
        }
    }
}