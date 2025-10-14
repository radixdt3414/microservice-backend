namespace order.Application.Order.Commands.OrderDelete
{
    public record OrderDeleteCommand(Guid Id) : ICommand<OrderDeleteResponse>
    {
    }

    public class OrderDeleteCommandValidator : AbstractValidator<OrderDeleteCommand>
    {
        public OrderDeleteCommandValidator()
        {
            RuleFor(x => x.Id).NotEmpty().WithMessage("Order Id is required");
        }
    }

    public record OrderDeleteResponse(bool IsSuccess);
}
