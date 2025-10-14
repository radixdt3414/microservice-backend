using order.Domain.ValueObjects;

namespace order.Application.Order.Commands.OrderFailed
{
    public record OrderFailedCommand : ICommand<OrderFailedResponse>
    {
        public OrderId Id { get; set; }
    }

    public class OrderFailedCommandValidator : AbstractValidator<OrderFailedCommand>
    {
        public OrderFailedCommandValidator()
        {
            RuleFor(x => x.Id).NotNull().WithMessage("Order Id is required");
        }
    }

    public record OrderFailedResponse { public bool IsSuccess { get; set; } };
}
