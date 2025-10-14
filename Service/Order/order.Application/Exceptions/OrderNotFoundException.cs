namespace order.Application.Exceptions
{
    public class OrderNotFoundException : NotFoundException
    {
        public OrderNotFoundException(Guid id) : base(id, "Order")
        {
        }
    }
}
