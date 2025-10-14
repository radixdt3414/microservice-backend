using authentication.API.Data;
using authentication.API.Exception;
using authentication.API.Services;
using buildingBlock.CQRS;
using FluentValidation;

namespace authentication.API.User.Login
{

    public record LoginCommand : ICommand<LoginResponse>
    {
        public string UserName { get; set; } = default!;
        public string Password { get; set; } = default!;
    }

    public class LoginValidator : AbstractValidator<LoginCommand>
    {
        public LoginValidator() { 
            RuleFor(x => x.UserName).NotEmpty().WithMessage("UserName is required");
            RuleFor(x => x.Password).NotEmpty().WithMessage("Password is required");
            RuleFor(x => x.Password).Matches(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^\da-zA-Z]).{8,}$").WithMessage("Password must be at least 8 characters long and contain at least one uppercase letter, one lowercase letter, one digit, and one special character");
        }
    }

    public record LoginResponse
    {
        public string Token { get; set; } = default!;
    }

    public class LoginCommandHandler(UserContext context, IPasswordHasher passwordHasher, JwtTokenGenerator jwtTokenGenerator) : ICommandHandler<LoginCommand, LoginResponse>
    {
        public async Task<LoginResponse> Handle(LoginCommand command, CancellationToken cancellationToken)
        {
            var user = context.Users.FirstOrDefault(u => u.UserName == command.UserName);

            if (user == null)
            {
                throw new UserNotFoundException(command.UserName);
            }

            if (!passwordHasher.VerifyPassword(command.Password, user.Salt ,user.Password))
            {
                throw new InvalidPasswordException();
            }
            string token = jwtTokenGenerator.GenerateToken(user);
            return new LoginResponse() { 
                Token = token
            };
        }
    }
}
