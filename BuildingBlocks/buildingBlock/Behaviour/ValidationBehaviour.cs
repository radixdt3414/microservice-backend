using buildingBlock.CQRS;
using buildingBlock.Exceptions;
using FluentValidation;
using MediatR;

namespace buildingBlock.Behaviour
{
    public class ValidationBehaviour<TRequest, TResponse>(IEnumerable<IValidator<TRequest>> validator) : IPipelineBehavior<TRequest, TResponse>
        where TRequest : ICommand<TResponse>
    {
        public async Task<TResponse> Handle(TRequest command, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            var context = new ValidationContext<TRequest>(command);
            //foreach (var item in validator)
            //{
            //    await item.ValidateAsync(context, cancellationToken);
            //}
            var validationResponse = await Task.WhenAll(validator.Select(async x => await x.ValidateAsync(context, cancellationToken)));
            var errorResponse = validationResponse.Where(x => x.Errors.Any()).SelectMany(x => x.Errors).ToList();
            if (errorResponse.Any())
            {
                throw new BadRequestException(new ValidationException(errorResponse));
            }
            return await next.Invoke(cancellationToken);
        }
    }
}