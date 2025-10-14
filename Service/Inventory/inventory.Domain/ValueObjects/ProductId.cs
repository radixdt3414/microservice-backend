using inventory.Domain.Exceptions;

namespace inventory.Domain.ValueObjects
{
    public class ProductId
    {
        public Guid Value { get; set; } = default!;
        private ProductId(Guid val) => Value = val;
        public static ProductId Of(Guid id)
        {
            ArgumentNullException.ThrowIfNull(id);
            if (id == Guid.Empty)
            {
                throw new DomainNullException("Product id couldn't be null or empty");
            }

            return new ProductId(id);
        }
    }
}
