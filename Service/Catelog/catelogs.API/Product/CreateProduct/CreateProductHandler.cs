using buildingBlock.Messaging;
using buildingBlock.Messaging.Events;
using FluentValidation;
using MassTransit;

namespace catelogs.API.Product.CreateProduct
{
    public record CreateProductCommand(string Name, string Description, decimal Price, string Image, List<string> Categories) : ICommand<CreateProductResponse>;
    public record CreateProductResponse(Guid Id);

    public class CreateProductCommandValidator : AbstractValidator<CreateProductCommand>
    {
        public CreateProductCommandValidator()
        {
            RuleFor(x => x.Name).NotEmpty().WithMessage("Name is required.");
            RuleFor(x => x.Description).NotEmpty().WithMessage("Description is required.");
            RuleFor(x => x.Price).GreaterThan(0).WithMessage("Price should be more then 0.");
            RuleFor(x => x.Image).NotEmpty().WithMessage("Image is required.");
            RuleFor(x => x.Categories).NotNull().NotEmpty().WithMessage("Categories are required.");
            RuleForEach(x => x.Categories).NotEmpty().WithMessage("Category cannot be empty.");
        }
    }

    public class CreateProductHandler(IDocumentSession session, IPublishEndpoint publisher) : ICommandHandler<CreateProductCommand, CreateProductResponse>
    {
        public async Task<CreateProductResponse> Handle(CreateProductCommand command, CancellationToken cancellationToken)
        {
            ProductModel product = new ProductModel()
            {
                Name = command.Name,
                Description = command.Description,
                Price = command.Price,
                Image = command.Image,
                Categories = command.Categories
            };
            session.Store(product);
            await session.SaveChangesAsync();
            await publisher.Publish(new CreateProductEvent
            {
                Id = product.Id,
                Name = command.Name,
                Price = command.Price,
            });
            return new CreateProductResponse(product.Id);
        }
    }

    //public class CreateProductHandler(IDocumentSession session, IValidator<CreateProductCommand> validator) : ICommandHandler<CreateProductCommand, CreateProductResponse>
    //{
    //    public async Task<CreateProductResponse> Handle(CreateProductCommand command, CancellationToken cancellationToken)
    //    {

    //        var validateResponse = await validator.ValidateAsync(command);
    //        if (!validateResponse.IsValid)
    //        {
    //            var errorObj = validateResponse.Errors
    //                .ToDictionary(err => err.PropertyName, err => err.ErrorMessage);

    //            throw new ValidationException(validateResponse.Errors);
    //        }

    //        ProductModel product = new ProductModel() {
    //            Name = command.Name,
    //            Description = command.Description,
    //            Price = command.Price,
    //            Image = command.Image,
    //            Categories = command.Categories
    //        };
    //        session.Store(product);
    //        await session.SaveChangesAsync();
    //        return new CreateProductResponse(product.Id);
    //    }
    //}
}