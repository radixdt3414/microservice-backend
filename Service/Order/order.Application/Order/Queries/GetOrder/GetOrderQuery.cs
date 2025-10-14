using buildingBlock.DTO;

namespace order.Application.Order.Queries.GetOrder
{
    public record GetOrderQuery(PaginationDTO PageDetails) : IQuery<GetOrderResponse>
    {
    }

    public record GetOrderResponse(PageResultDTO<IEnumerable<OrderDTO>> OrderList) { }
}
