using Microsoft.AspNetCore.Mvc;
using order.Application.Order.Queries.GetOrder;
using order.Application.Order.Queries.GetOrderByCustomer;

namespace order.API.Endpoint
{
    public record GetOrderByCustomerRequestDTO(Guid CustomerId) { }
    public record GetOrderByCustomerResponseDTO(IEnumerable<OrderDTO> OrderList) { }

    public class GetOrderByCustomer : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapPost("/order/customer", async ([FromBody]GetOrderByCustomerRequestDTO request, ISender _mediator) =>
            {
                var command = request.Adapt<GetOrderByCustomerQuery>();
                var response = await _mediator.Send(command);
                var result = response.Adapt<GetOrderByCustomerResponseDTO>();
                return Results.Ok(result);
            })
            .WithSummary("Get order by customer id")
            .WithDescription("Get order by customer id")
            .Produces(200, typeof(GetOrderByCustomerResponseDTO))
            .ProducesProblem(400, null);
        }
    }
}