namespace order.Domain.ValueObjects
{
    public record OrderItemId
    {
        public Guid Value { get; private set; }

        private OrderItemId(Guid val) => Value = val;

        public static OrderItemId of(Guid val)
        {
            ArgumentNullException.ThrowIfNull(val);
            if (val == Guid.Empty)
            {
                throw new DomainException("Order Item Id can't be empty.");
            }
            return new OrderItemId(val);
        }
    }
}
