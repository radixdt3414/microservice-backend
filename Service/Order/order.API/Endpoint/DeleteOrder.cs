using Microsoft.AspNetCore.Mvc;
using order.Application.Order.Commands.OrderCreate;
using order.Application.Order.Commands.OrderDelete;

namespace order.API.Endpoint
{

    public record DeleteOrderRequestDTO(Guid Id) { }
    public record DeleteOrderResponseDTO(bool IsSuccess) { }

    public class DeleteOrder : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapDelete("/order", async ([FromBody]DeleteOrderRequestDTO request, ISender _mediator) =>
            {
                var command = request.Adapt<OrderDeleteCommand>();
                var response = await _mediator.Send(command);
                var result = response.Adapt<DeleteOrderResponseDTO>();
                return result.IsSuccess ?
                        Results.Ok("Order deleted successfully")
                        : Results.Problem(
                            title: "Order deletion failed.",
                            detail: "While deleting order error encountered.",
                            statusCode: StatusCodes.Status500InternalServerError
                            );
            })
            .WithSummary("Delete order")
            .WithDescription("Delete order")
            .Produces(200, typeof(DeleteOrderResponseDTO))
            .ProducesProblem(500, null);
        }
    }
}