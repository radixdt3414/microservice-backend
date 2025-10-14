namespace catelogs.API.Product.DeleteProduct
{
    public record DeleteProductResponseDTO(bool IsSuccess);

    public class DeleteProductEndpoint : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapDelete("/product/{Id}", async (Guid Id, IMediator _mediator) =>
            {
                var command = new DeleteProductCommand(Id);
                var response = await _mediator.Send(command);
                var result = response.Adapt<DeleteProductResponseDTO>();
                return Results.Ok(result);
            })
            .WithSummary("Delete product by id")
            .WithDescription("Delete product by id")
            .Produces(200, typeof(DeleteProductResponseDTO))
            .ProducesProblem(400, null);
        }
    }
}
