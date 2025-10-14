using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace buildingBlock.CQRS
{
    public interface IQueryHandler<in TRequest, TResponse> : IRequestHandler<TRequest, TResponse> 
        where TRequest : IQuery<TResponse>
        where TResponse: class
    {
    }
}
