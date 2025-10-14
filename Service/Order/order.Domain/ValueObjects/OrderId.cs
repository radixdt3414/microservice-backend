namespace order.Domain.ValueObjects
{
    public record OrderId
    {
        public Guid Value { get; private set; }

        private OrderId(Guid val) => Value = val;

        public static OrderId of(Guid val)
        {
            ArgumentNullException.ThrowIfNull(val);
            if(val == Guid.Empty)
            {
                throw new DomainException("Order Id can't be null or empty.");
            }
            return new OrderId(val);
        }

    }
}