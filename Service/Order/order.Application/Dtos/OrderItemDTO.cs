using order.Domain.ValueObjects;

namespace order.Application.Dtos
{
    public record OrderItemDTO
    {
        public OrderItemDTO() { }
        public OrderItemDTO(
            Guid? _OrderId,
            long _Quentity,
            Guid _ProductId,
            decimal _Price,
            string? _ProductName
            )
        {
            OrderId = _OrderId;
            Quentity = _Quentity;
            ProductId = _ProductId;
            Price = _Price;
            ProductName = _ProductName;

        }

        public Guid? OrderId { get; set; }
        public long Quentity { get; set; }
        public Guid ProductId { get; set; }
        public decimal Price { get; set; }
        public string? ProductName { get; set; }
    }
           
}