namespace order.Domain.Models
{
    public class Product : Entity<ProductId>
    {
        public string ProductName { get; private set; } = default!;
        public decimal Price { get; private set; } = default!;

        public static Product Create(ProductId Id, string ProductName, decimal Price)
        {
            ArgumentNullException.ThrowIfNull(nameof(Id));
            ArgumentException.ThrowIfNullOrWhiteSpace(nameof(ProductName));
            ArgumentNullException.ThrowIfNull(nameof(Price));
            ArgumentOutOfRangeException.ThrowIfZero(Price);
            return new Product()
            {
                Id = Id,
                ProductName = ProductName,
                Price = Price,
                CreatedDate = DateTime.Now,
                ModifiedDate = DateTime.Now
            };
        }
    }
}