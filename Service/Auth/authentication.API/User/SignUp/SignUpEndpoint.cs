using Carter;
using Mapster;
using MediatR;

namespace authentication.API.User.SignUp
{

    public record SignUpRequestDTO
    {
        public string FirstName { get; set; } = default!;
        public string LastName { get; set; } = default!;
        public string UserName { get; set; } = default!;
        public string Email { get; set; } = default!;
        public string Password { get; set; } = default!;

    }

    public record SignUpResponseDTO
    {
        public bool IsSuccess { get; set; } = default!;
    }

    public class SignUpEndpoint : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapPost("signup", async (SignUpRequestDTO request, ISender sender) =>
            {
                var command = request.Adapt<SignUpCommand>();
                var result = await sender.Send(command);
                var response = result.Adapt<SignUpResponseDTO>();
                return response.IsSuccess ? Results.Ok(response) : Results.Problem(detail: "While signing up, something went wrong", title: "Signup failed", statusCode:500);
            });
        }
    }
}
