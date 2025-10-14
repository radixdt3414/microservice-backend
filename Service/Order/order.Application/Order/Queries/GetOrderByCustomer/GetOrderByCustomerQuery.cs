namespace order.Application.Order.Queries.GetOrderByCustomer
{
    public record GetOrderByCustomerQuery(Guid CustomerId) : IQuery<GetOrderByCustomerResponse>
    {
    }

    public record GetOrderByCustomerResponse(IEnumerable<OrderDTO> OrderList) { }
}
