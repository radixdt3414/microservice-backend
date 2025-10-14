
using basket.API.Cart.AddItem;
using basket.API.Dtos;

namespace basket.API.Cart.Checkout
{

    public record CheckoutRequestDTO(CheckoutDetailsDTO CheckoutDetails) { };

    public record CheckoutResponseDTO
    {
        public bool IsSuccess { get; set; }
    }

    public class CheckoutEndpoint : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapPost("/basket/checkout", async (CheckoutRequestDTO request, ISender sender) =>
            {
                var req = request.Adapt<CheckoutCommand>();
                var response = await sender.Send(req);
                var result = response.Adapt<CheckoutResponseDTO>();
                return result.IsSuccess ? Results.Ok(new CheckoutResponseDTO { IsSuccess = true})
                        : Results.Problem(
                            title: "Checkout failed.",
                            detail: "While order item, error encountered.",
                            statusCode: StatusCodes.Status500InternalServerError
                            );
            });
        }
    }
}
