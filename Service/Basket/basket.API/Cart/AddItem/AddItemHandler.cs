using basket.API.Data.CartCacheStore;
using basket.API.Models;
using discount.API.Protos;
using FluentValidation;
using System.Text.Json;

namespace basket.API.Cart.AddItem
{
    public record AddItemCommand(CartModel Cart) : ICommand<AddItemResponse>;
    public record AddItemResponse();
    public class AddItemCommandValidator : AbstractValidator<AddItemCommand>
    {
        public AddItemCommandValidator()
        {
            RuleFor(x => x.Cart).NotNull().WithMessage("Please add cart details").SetValidator(new CartModelValidator());
        }
    }
    public class CartModelValidator : AbstractValidator<CartModel>
    {
        public CartModelValidator()
        {
            RuleFor(x => x.UserName).NotNull().NotEmpty().WithMessage("Please add user name");
            RuleFor(x => x.Items).NotNull().WithMessage("Please add Item to cart");
            RuleForEach(x => x.Items).SetValidator(new CartitemValidator());
        }
    }

    public class CartitemValidator : AbstractValidator<CartItem>
    {
        public CartitemValidator()
        {
            RuleFor(x => x.Quentity).GreaterThan(0).WithMessage("Please add atleast one item.");
            RuleFor(x => x.ProductName).NotEmpty().WithMessage("Please add product name.");
            RuleFor(x => x.Price).GreaterThan(0).WithMessage("Price should not be zero.");
        }
    }

    public class AddItemHandler(ICartItemRepository cartRepository, ICacheService<CartModel> cache, DiscountProtoService.DiscountProtoServiceClient discountService, ILogger<AddItemHandler> logger) : ICommandHandler<AddItemCommand, AddItemResponse>
    {
        public async Task<AddItemResponse> Handle(AddItemCommand request, CancellationToken cancellationToken)
        {
            logger.LogInformation($"--------------------basket------> OrderFailedEventConsumer: AddItemHandler invoked.");

            CartModel? cart = await cartRepository.GetCart(request.Cart.UserName);

            foreach(var item in request.Cart.Items)
            {
                var discountItem = discountService.GetDiscount(new GetDiscountRequest() { ProductName = item.ProductName });
                logger.LogInformation($"--------------------basket------> OrderFailedEventConsumer: discountPrice:{(discountItem?.DiscountPrice ?? 0)} ,  discount item: { JsonSerializer.Serialize(discountItem)}  <-----------");
                item.Price -= (discountItem?.DiscountPrice ?? 0); 
            }
            request.Cart.TotalPrice = request.Cart.Items.Sum(x => x.Price * x.Quentity);
            if (cart == null)
            {
                logger.LogInformation("-----basket------> OrderFailedEventConsumer: Cart not found: will be added new <-----------");
                await cartRepository.AddCart(request.Cart, cancellationToken);
            }
            else
            {
                logger.LogInformation("-----basket------> OrderFailedEventConsumer: Cart found: will be updated <-----------");
                request.Cart.Id = cart.Id;
                await cartRepository.UpdateCart(request.Cart, cancellationToken);
                //await cartRepository.AddCart(cart, cancellationToken); update cart
            }
            //Below cacheing functionality has already taken care by decortor pattern
            //await cache.Set(request.Cart.UserName, request.Cart, DateTime.UtcNow.AddMinutes(5), TimeSpan.FromMinutes(2));
            return new AddItemResponse();
        }
    }
}