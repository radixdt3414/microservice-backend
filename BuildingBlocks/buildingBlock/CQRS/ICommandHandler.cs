using MediatR;
using Microsoft.AspNetCore.Identity.Data;

namespace buildingBlock.CQRS
{
    public interface ICommandHandler<in TRequest,  TResponse> : IRequestHandler<TRequest, TResponse> 
        where TRequest : ICommand<TResponse>
        where TResponse : notnull
    {
    }
}
