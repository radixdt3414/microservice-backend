using Microsoft.AspNetCore.Mvc;
using order.Application.Order.Commands.OrderCreate;
using order.Application.Order.Commands.OrderUpdate;

namespace order.API.Endpoint
{
    public record UpdateOrderRequestDTO(OrderDTO Order) { }
    public record UpdateOrderResponseDTO(bool IsSuccess) { }
    public class UpdateOrder : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapPut("/order", async ([FromBody] UpdateOrderRequestDTO request, ISender _mediator) =>
            {
                var command = request.Adapt<OrderUpdateCommand>();
                var response = await _mediator.Send(command);
                var result = response.Adapt<UpdateOrderResponseDTO>();
                return result.IsSuccess ? 
                        Results.Ok("Order updated successfully") 
                        : Results.Problem(
                            title: "Order updation failed.",
                            detail: "While updating order error encountered.", 
                            statusCode: StatusCodes.Status500InternalServerError
                            );
            })
            .WithSummary("Update order")
            .WithDescription("Update order")
            .Produces(200, typeof(UpdateOrderRequestDTO))
            .ProducesProblem(500, null);
        }
    }
}