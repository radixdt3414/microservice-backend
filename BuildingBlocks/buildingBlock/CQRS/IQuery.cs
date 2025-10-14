using MediatR;

namespace buildingBlock.CQRS
{
    public interface IQuery<out TResponse> : IRequest<TResponse> where TResponse : class
    {
    }
}
