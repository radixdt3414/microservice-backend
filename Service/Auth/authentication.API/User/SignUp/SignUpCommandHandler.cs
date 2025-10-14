using authentication.API.Data;
using authentication.API.Exception;
using authentication.API.Services;
using authentication.API.User.Login;
using buildingBlock.CQRS;
using buildingBlock.Messaging.Events;
using FluentValidation;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using UserModel = authentication.API.Models.User;    

namespace authentication.API.User.SignUp
{

    public record SignUpCommand : ICommand<SignUpResponse>
    {
        public string FirstName { get; set; } = default!;
        public string LastName { get; set; } = default!;
        public string UserName { get; set; } = default!;
        public string Email { get; set; } = default!;
        public string Password { get; set; } = default!;
    }

    public class SignUpCommandValidator : AbstractValidator<SignUpCommand>
    {
        public SignUpCommandValidator()
        {
            RuleFor(x => x.FirstName).NotEmpty().WithMessage("FirstName is required");
            RuleFor(x => x.LastName).NotEmpty().WithMessage("Lastname is required");
            RuleFor(x => x.Email).NotEmpty().WithMessage("Email is required").EmailAddress().WithMessage("Email is not valid");
            RuleFor(x => x.UserName).NotEmpty().WithMessage("UserName is required");
            RuleFor(x => x.Password).NotEmpty().WithMessage("Password is required").Matches(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^\da-zA-Z]).{8,}$").WithMessage("Password must be at least 8 characters long and contain at least one uppercase letter, one lowercase letter, one digit, and one special character");
        }
    }

    public record SignUpResponse
    {
        public bool IsSuccess { get; set; } = default!;
    }
    public class SignUpCommandHandler(UserContext context, IPasswordHasher passwordHasher, IPublishEndpoint publisher) : ICommandHandler<SignUpCommand, SignUpResponse>
    {
        public async Task<SignUpResponse> Handle(SignUpCommand command, CancellationToken cancellationToken)
        {
            var user = await context.Users.Where(x => x.UserName == command.UserName || x.Email == command.Email).FirstOrDefaultAsync();
            if (user != null)
            {
                throw new UserAlreadyExistException(command.UserName);
            }
            string hasedSalt; string hashedPassword;
            passwordHasher.HashPassword(command.Password, out hasedSalt, out hashedPassword);

            user = new UserModel
            {
                FirstName = command.FirstName,
                LastName = command.LastName,
                UserName = command.UserName,
                Email = command.Email,
                Password = hashedPassword,
                Salt = hasedSalt
            };

            context.Users.Add(user);
            await context.SaveChangesAsync();
            await publisher.Publish(new CustomerCreateEvent()
            {
                Id = user.Id,
                UserName = user.UserName,
                Email = user.Email
            });
            return new SignUpResponse { IsSuccess = true };
        }
    }
}
