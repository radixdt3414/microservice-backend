namespace order.Domain.ValueObjects
{
    public record ProductId
    {
        public Guid Value { get; private set; }

        private ProductId(Guid val) => Value = val;

        public static ProductId of(Guid val)
        {
            ArgumentNullException.ThrowIfNull(val);
            if (val == Guid.Empty)
            {
                throw new DomainException("Product Id can't be empty.");
            }
            return new ProductId(val);
        }
    }
}