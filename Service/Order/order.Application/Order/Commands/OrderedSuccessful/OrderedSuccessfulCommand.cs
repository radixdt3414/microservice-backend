using order.Domain.ValueObjects;

namespace order.Application.Order.Commands.OrderedSuccessful
{
    public record OrderedSuccessfulCommand(OrderId id) : ICommand<OrderedSuccessfulResponse>;
    
    public class OrderedSuccessfulCommandValidator : AbstractValidator<OrderedSuccessfulCommand>
    {
        public OrderedSuccessfulCommandValidator()
        {
            RuleFor(x => x.id).NotNull().WithMessage("Order Id is required");
        }
    }
    
    public record OrderedSuccessfulResponse(bool IsSuccess);
}
