namespace catelogs.API.Product.CreateProduct
{
    public record CreateProductRequestDTO(string Name, string Description, decimal Price, string Image, List<string> Categories);

    public record CreateProductResponseDTO(Guid Id);

    public class CreateProductEndpoint : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapPost("/product", async (CreateProductRequestDTO request, IMediator _mediator) =>
            {
                var command = request.Adapt<CreateProductCommand>();
                var response = await _mediator.Send(command);
                var result = response.Adapt<CreateProductResponseDTO>();
                return Results.Created($"/catelog/{result.Id}", result);
            })
            .WithSummary("Create product")
            .WithDescription("Create product")
            .Produces(201, typeof(CreateProductResponseDTO))
            .ProducesProblem(400, null);
        }
    }
}