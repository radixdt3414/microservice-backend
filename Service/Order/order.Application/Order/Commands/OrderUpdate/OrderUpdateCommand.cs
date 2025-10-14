namespace order.Application.Order.Commands.OrderUpdate
{
    public record OrderUpdateCommand(OrderDTO Order) : ICommand<OrderUpdateResponse>
    {
    }

    public class OrderUpdateCommandValidator : AbstractValidator<OrderUpdateCommand>
    {
        public OrderUpdateCommandValidator()
        {
            RuleFor(x => x.Order.CustomerId).NotEmpty().NotNull().WithMessage("Customer Id shouldn't be empty or null");
            RuleFor(x => x.Order.OrderName).NotEmpty().NotNull().WithMessage("Order name shouldn't be empty or null");
            RuleFor(x => x.Order.OrderItems).NotNull().NotEmpty().WithMessage("Order items are required");
            RuleForEach(x => x.Order.OrderItems).ChildRules(item => {
                item.RuleFor(x => x.ProductId).NotEmpty().WithMessage("Product Id is required");
                item.RuleFor(x => x.Quentity).GreaterThan(0).WithMessage("Quantity must be greater than 0");
                item.RuleFor(x => x.Price).GreaterThan(0).WithMessage("Price must be greater than 0");
            });
            RuleFor(x => x.Order.PaymentDetails).NotNull().WithMessage("Payment details are required");
            RuleFor(x => x.Order.PaymentDetails.CardNumber).NotEmpty().WithMessage("Card number is required");
            RuleFor(x => x.Order.PaymentDetails.Cvv).NotEmpty().WithMessage("CVV is required");
            RuleFor(x => x.Order.PaymentDetails.CardMemberName).NotEmpty().WithMessage("Card member name is required");
            RuleFor(x => x.Order.OrderAddressDetails).NotNull().WithMessage("Order address is required");
            RuleFor(x => x.Order.OrderAddressDetails.FirstName).NotEmpty().WithMessage("First name is required");
            RuleFor(x => x.Order.OrderAddressDetails.LastName).NotEmpty().WithMessage("Last name is required");
            RuleFor(x => x.Order.ShippingAddressDetails).NotNull().WithMessage("Shipping address is required");
        }
    }

    public record OrderUpdateResponse(bool IsSuccess);
}
