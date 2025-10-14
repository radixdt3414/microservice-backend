using Microsoft.Extensions.Logging;
using order.Application.Order.Commands.OrderCreate;
using order.Domain.ValueObjects;
using ProductModel = order.Domain.Models.Product;

namespace order.Application.Products.Commands.CreateProduct
{
    public record CreateProductCommand : ICommand<CreateProductResponse>
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = default!;
        public decimal Price { get; set; } = default!;
    }

    public record CreateProductResponse(bool IsSuccess);

    public class CreateProductCommandValidator : AbstractValidator<CreateProductCommand>
    {
        public CreateProductCommandValidator()
        {
            RuleFor(x => x.Id).NotEmpty().WithMessage("Product Id is required");
            RuleFor(x => x.Name).NotEmpty().WithMessage("Product name is required");
            RuleFor(x => x.Price).GreaterThan(0).WithMessage("Price must be greater than 0");
        }
    }

    public class CreateProductCommandHandler(IApplicationDbContext DbContext,
            ILogger<OrderCreateCommandHandler> logger) : ICommandHandler<CreateProductCommand, CreateProductResponse>
    {
        public async Task<CreateProductResponse> Handle(CreateProductCommand command, CancellationToken cancellationToken)
        {
            ProductModel product = ProductModel.Create(ProductId.of(command.Id),command.Name, command.Price);
            await DbContext.Products.AddAsync(product);
            await DbContext.SaveChangesAsync();
            return new CreateProductResponse(true);
        }
    }
}
