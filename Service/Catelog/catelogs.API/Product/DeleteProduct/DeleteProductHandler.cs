
using buildingBlock.Messaging.Events;
using catelogs.API.Exceptions;
using catelogs.API.Product.GetByIdProduct;
using FluentValidation;
using MassTransit;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace catelogs.API.Product.DeleteProduct
{
    public record DeleteProductCommand(Guid Id): ICommand<DeleteProductResponse>;

    public record DeleteProductResponse(bool IsSuccess);

    public class DeleteProductCommandValidator : AbstractValidator<DeleteProductCommand>
    {
        public DeleteProductCommandValidator()
        {
            RuleFor(x => x.Id).NotEmpty().WithMessage("Product Id is required");
        }
    }

    public class DeleteProductHandler(IDocumentSession session, IPublishEndpoint Publisher) : ICommandHandler<DeleteProductCommand, DeleteProductResponse>
    {
        public async Task<DeleteProductResponse> Handle(DeleteProductCommand command, CancellationToken cancellationToken)
        {
            var response = await session.Query<ProductModel>().FirstOrDefaultAsync(x => x.Id == command.Id);
            if (response == null)
            {
                throw new ProductNotFoundException(command.Id);
            }
            session.Delete(response);
            await session.SaveChangesAsync();
            await Publisher.Publish(new DeleteProductEvent
            {
                Id =  command.Id
            });
            return new DeleteProductResponse(true);
        }
    }
}
