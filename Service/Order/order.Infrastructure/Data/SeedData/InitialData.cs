using order.Domain.Models;
using order.Domain.ValueObjects;

namespace order.Infrastructure.Data.SeedData
{
    public class InitialData
    {
        public static List<Customer> CustomerData = new List<Customer>(){
            Customer.Create(
                CustomerId.of(new Guid("a3f6c9c7-12e4-4a57-b0dd-9d9c1c5a8b92")),
                "john.doe",
                "john.doe@example.com"
            ),

            Customer.Create(
                CustomerId.of(new Guid("7b2a1d44-8f03-4e21-9c56-1b5d4f30219a")),
                "jane.smith",
                "jane.smith@example.com"
            ),

            Customer.Create(
                CustomerId.of(new Guid("c9d8e76a-3f1b-48f2-84b7-1a6d44a2c7dd")),
                "mike.ross",
                "mike.ross@example.com"
            ),
        };

        public static List<Product> ProductData = new List<Product>()
        {
            Product.Create(
                ProductId.of(new Guid("019933d8-d5cb-463f-911d-1ce871e00627")),
                "Samsung Galaxy S24 Ultra",
                (decimal)105000.00),

            Product.Create(
                ProductId.of(new Guid("01992e66-3ebd-4dc9-b411-3551d5f7f5a5")),
                "Lenovo ThinkPad X1 Carbon",
                (decimal)120000.00),

            Product.Create(
                ProductId.of(new Guid("01992e66-3ebc-4490-aa56-67ae4ecc572d")),
                "HP Pavilion",
                (decimal)55000.00),
        };

        public static List<Order> OrderAndOrderItemData 
        {
            get {
                Order order = Order.Create(
                    OrderId.of(new Guid("4e2b1d77-9c8f-45b1-b0a4-2f73a1e5d6c8")),
                    Address.of(
                                        "Priya",
                                        "Sharma",
                                        "India",
                                        "Opposite City Mall",
                                        "Maharashtra",
                                        "Mumbai",
                                        "400001",
                                        "Flat 7A, Sunrise Apartments"
                                    ),
                    Address.of(
                                        "John",
                                        "Doe",
                                        "USA",
                                        "Near Central Park",
                                        "New York",
                                        "New York City",
                                        "10001",
                                        "Apartment 12B"
                                    ),
                    OrderName.of("Festive offers"),
                    CustomerId.of(new Guid("a3f6c9c7-12e4-4a57-b0dd-9d9c1c5a8b92")),
                     Payment.of(
                        "4111111111111111",   // Visa test card number
                        "123",
                        new DateTime(2026, 12, 31),
                        "CreditCard",
                        "John"
                    ),Domain.Enum.OrderStatus.Processing);
                order.AddItem(2, ProductId.of(new Guid("019933d8-d5cb-463f-911d-1ce871e00627")), (decimal)105000.00);
                order.AddItem(1, ProductId.of(new Guid("01992e66-3ebd-4dc9-b411-3551d5f7f5a5")), (decimal)120000.00);
                return new List<Order>{ order };
            }
        }
    }
}