using catelogs.API.Product.CreateProduct;

namespace catelogs.API.Product.UpdateProduct
{
    public record UpdateProductRequestDTO(Guid Id, string Name, string Description, decimal Price, string Image, List<string> Categories);

    public class UpdateProductEndpoint : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapPut("/product", async (UpdateProductRequestDTO request, IMediator _mediator) =>
            {
                var command = request.Adapt<UpdateProductCommand>();
                var response = await _mediator.Send(command);
                if (!response.IsSuccess)
                {
                    throw new Exception("Record update failed!");
                }
                return Results.Ok("Record updated successfully.");
            })
            .WithSummary("Update product")
            .WithDescription("Update product")
            .Produces(201, typeof(CreateProductResponseDTO))
            .ProducesProblem(400, null);
        }
    }
}
