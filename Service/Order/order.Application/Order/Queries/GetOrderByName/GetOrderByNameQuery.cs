namespace order.Application.Order.Queries.GetOrderByName
{
    public record GetOrderByNameQuery(string name) : IQuery<GetOrderByNameResponse>
    {
    }

    public record GetOrderByNameResponse(IEnumerable<OrderDTO> OrderList) { }
}
