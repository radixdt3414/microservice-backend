using catelogs.API.Exceptions;
using FluentValidation;

namespace catelogs.API.Product.UpdateProduct
{
    public record UpdateProductCommand(Guid Id, string Name, string Description, decimal Price, string Image, List<string> Categories) : ICommand<UpdateProductResponse>;
    public record UpdateProductResponse(bool IsSuccess);

    public class UpdateProductCommandValidator : AbstractValidator<UpdateProductCommand>
    {
        public UpdateProductCommandValidator()
        {
            RuleFor(x => x.Id).NotEmpty().WithMessage("Product Id is required");
            RuleFor(x => x.Name).NotEmpty().WithMessage("Name is required");
            RuleFor(x => x.Description).NotEmpty().WithMessage("Description is required");
            RuleFor(x => x.Price).GreaterThan(0).WithMessage("Price must be greater than 0");
            RuleFor(x => x.Image).NotEmpty().WithMessage("Image is required");
            RuleFor(x => x.Categories).NotNull().NotEmpty().WithMessage("Categories are required");
            RuleForEach(x => x.Categories).NotEmpty().WithMessage("Category cannot be empty");
        }
    }

    public class UpdateProductHandler(IDocumentSession session) : ICommandHandler<UpdateProductCommand, UpdateProductResponse>
    {
        public async Task<UpdateProductResponse> Handle(UpdateProductCommand command, CancellationToken cancellationToken)
        {
            var product =await session.LoadAsync<ProductModel>(command.Id, cancellationToken);
            if(product == null)
            {
                throw new ProductNotFoundException(command.Id);
            }

            product.Name = command.Name;
            product.Description = command.Description;
            product.Price = command.Price;
            product.Image = command.Image;
            product.Categories = command.Categories;

            session.Update(product);
            await session.SaveChangesAsync(cancellationToken);
            return new UpdateProductResponse(true);
        }
    }
}