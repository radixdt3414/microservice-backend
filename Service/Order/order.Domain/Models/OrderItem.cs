namespace order.Domain.Models
{
    public class OrderItem : Entity<OrderItemId>
    {
        public OrderId OrderId { get; set; }
        public long Quentity { get; private set; } = default!;
        public ProductId ProductId { get; private set; } = default!;
        public decimal Price { get; private set; } = default!;

        public static OrderItem Create(OrderId _OrderId, long _Quentity, ProductId _ProductId, decimal _Price)
        {
            ArgumentOutOfRangeException.ThrowIfLessThan(_Quentity,1);
            ArgumentOutOfRangeException.ThrowIfLessThan(_Price,1);
            var obj = new OrderItem
            {
                Id = OrderItemId.of(Guid.NewGuid()),
                Quentity = _Quentity,
                ProductId = _ProductId,
                OrderId = _OrderId,
                Price = _Price,
                CreatedDate = DateTime.Now,
                ModifiedDate = DateTime.Now
            };
            return obj;
        }

        public void Update(long _Quentity, ProductId _ProductId, decimal _Price)
        {
            ArgumentOutOfRangeException.ThrowIfLessThan(_Quentity, 1);
            ArgumentOutOfRangeException.ThrowIfLessThan(_Price, 1);

            Quentity = _Quentity;
            Price = _Price;
        }
    }
}