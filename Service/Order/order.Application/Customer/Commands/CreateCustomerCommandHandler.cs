using buildingBlock.Messaging.Events;
using Microsoft.Extensions.Logging;
using order.Application.Order.Commands.OrderCreate;
using CustomerModel = order.Domain.Models.Customer;
using order.Domain.ValueObjects;

namespace order.Application.Customer.Commands
{

    public record CreateCustomerCommand : ICommand<CreateCustomerResponse>
    {
        public Guid Id { get; set; }
        public string UserName { get; set; } = default!;
        public string Email { get; set; } = default!;
    }

    public record CreateCustomerResponse
    {
        public bool IsSuccess { get; set; }
    }

    public class CreateCustomerCommandValidator : AbstractValidator<CreateCustomerCommand>
    {
        public CreateCustomerCommandValidator()
        {
            RuleFor(x => x.Id).NotEmpty().WithMessage("Id is required");
            RuleFor(x => x.UserName).NotEmpty().WithMessage("UserName is required");
            RuleFor(x => x.Email).NotEmpty().EmailAddress().WithMessage("Valid email is required");
        }
    }

    public class CreateCustomerCommandHandler(IApplicationDbContext DbContext, ILogger<OrderCreateCommandHandler> logger) : ICommandHandler<CreateCustomerCommand, CreateCustomerResponse>
    {
        public async Task<CreateCustomerResponse> Handle(CreateCustomerCommand command, CancellationToken cancellationToken)
        {
            var customer = CustomerModel.Create(CustomerId.of(command.Id),command.UserName, command.Email);
            DbContext.Customers.Add(customer);
            await DbContext.SaveChangesAsync();
            return new CreateCustomerResponse() { IsSuccess = true };
        }
    }
}
