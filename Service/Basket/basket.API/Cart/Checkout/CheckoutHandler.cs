using basket.API.Dtos;
using buildingBlock.Messaging.Events;
using FluentValidation;
using MassTransit;

namespace basket.API.Cart.Checkout
{

    public record CheckoutCommand(CheckoutDetailsDTO CheckoutDetails) : ICommand<CheckoutResponse>;

    public class CheckoutCommandValidator : AbstractValidator<CheckoutCommand>
    {
        public CheckoutCommandValidator()
        {
            RuleFor(x => x.CheckoutDetails.UserName).NotEmpty();
            RuleFor(x => x.CheckoutDetails.Shipping_City).NotEmpty();
            RuleFor(x => x.CheckoutDetails.Shipping_Country).NotEmpty();
            RuleFor(x => x.CheckoutDetails.Shipping_Description).NotEmpty();
            RuleFor(x => x.CheckoutDetails.Shipping_FirstName).NotEmpty();
            RuleFor(x => x.CheckoutDetails.Shipping_Landmark).NotEmpty();
            RuleFor(x => x.CheckoutDetails.Shipping_LastName).NotEmpty();
            RuleFor(x => x.CheckoutDetails.Shipping_PostalCode).NotEmpty();
            RuleFor(x => x.CheckoutDetails.Shipping_State).NotEmpty();
            RuleFor(x => x.CheckoutDetails.CardNumber).NotEmpty().Length(16);
            RuleFor(x => x.CheckoutDetails.Cvv).NotEmpty().Length(3, 4);
            RuleFor(x => x.CheckoutDetails.TotalPrice).GreaterThan(0);
            RuleFor(x => x.CheckoutDetails.Order_City).NotEmpty();
            RuleFor(x => x.CheckoutDetails.Order_Country).NotEmpty();
            RuleFor(x => x.CheckoutDetails.Order_Description).NotEmpty();
            RuleFor(x => x.CheckoutDetails.Order_FirstName).NotEmpty();
            RuleFor(x => x.CheckoutDetails.Order_Landmark).NotEmpty();
            RuleFor(x => x.CheckoutDetails.Order_LastName).NotEmpty();
            RuleFor(x => x.CheckoutDetails.Order_PostalCode).NotEmpty();
            RuleFor(x => x.CheckoutDetails.Order_State).NotEmpty();
            RuleFor(x => x.CheckoutDetails.PaymentType).NotEmpty();
            RuleFor(x => x.CheckoutDetails.CardMemberName).NotEmpty();
            RuleFor(x => x.CheckoutDetails.CustomerId).NotEmpty();
        }
    }

    public record CheckoutResponse(bool IsSuccess);
    public class CheckoutHandler(ICartItemRepository cartItemRepository, IPublishEndpoint publisher, ILogger<CheckoutHandler> logger) : ICommandHandler<CheckoutCommand, CheckoutResponse>
    {
        public async Task<CheckoutResponse> Handle(CheckoutCommand command, CancellationToken cancellationToken)
        {
            // find basket
            var cartObj = await cartItemRepository.GetCart(command.CheckoutDetails.UserName);
            if(cartObj == null)
            {
                return new CheckoutResponse(false);
            }

            // dispatch order create integration event 
            var checkoutObj = command.CheckoutDetails.Adapt<BasketCheckoutEvent>();
            checkoutObj.OrderName = "abc-xyz";
            checkoutObj.CorrelationId = Guid.NewGuid();
            logger.LogInformation($"CheckoutHandler: Created correlationId {checkoutObj.CorrelationId}");
            cartObj.Items.ForEach(item => {
                checkoutObj.items.Add(item.Adapt<BaskteItem>());
            });
            
            logger.LogInformation($"CheckoutHandler: Publish event BasketCheckoutEvent with data{System.Text.Json.JsonSerializer.Serialize(checkoutObj)}");
            await publisher.Publish(checkoutObj, cancellationToken);


            // delete basket
            await cartItemRepository.DeleteCart(cartObj);
            return new CheckoutResponse(true);
        }
    }
}
