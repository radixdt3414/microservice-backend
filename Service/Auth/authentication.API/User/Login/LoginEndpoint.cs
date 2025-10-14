using Carter;
using Mapster;
using MediatR;

namespace authentication.API.User.Login
{


    public record LoginRequestDTO 
    {
        public string UserName { get; set; } = default!;
        public string Password { get; set; } = default!;
    }

    public record LoginResponseDTO
    {
        public string Token { get; set; } = default!;
    }

    public class LoginEndpoint : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapPost("login", async (LoginRequestDTO request, ISender sender) =>
            {
                var command = request.Adapt<LoginCommand>();
                var result = await sender.Send(command);
                var response = result.Adapt<LoginResponseDTO>();
                return !string.IsNullOrWhiteSpace(response.Token) ? Results.Ok(response) : Results.Problem(detail: "While login, something went wrong", title: "Login failed", statusCode: 500);
            });
        }
    }
}
