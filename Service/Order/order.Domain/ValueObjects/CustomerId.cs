namespace order.Domain.ValueObjects
{
    public record CustomerId
    {
        public Guid Value { get; private set; }

        private CustomerId(Guid val) => Value = val;

        public static CustomerId of(Guid val)
        {
            ArgumentNullException.ThrowIfNull(val);
            if (val == Guid.Empty)
            {
                throw new DomainException("Customer Id can't be empty.");
            }
            return new CustomerId(val);
        }
    }
}