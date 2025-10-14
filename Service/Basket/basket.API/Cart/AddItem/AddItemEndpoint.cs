namespace basket.API.Cart.AddItem
{
    public record AddItemRequestDTO(CartModel Cart);
    
    public class AddItemEndpoint : CarterModule
    {
        public override void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapPost("/basket", async (AddItemRequestDTO request, ISender sender) =>
            {
                var req = request.Adapt<AddItemCommand>();
                var response = await sender.Send(req);
                return Results.Created("/basket",null);
            })
                .WithName("AddItem")
                .WithDescription("Add item to the baskte.")
                .WithSummary("Add item to the baskte.")
                .Produces(200)
                .ProducesProblem(400, null);
        }
    }
}