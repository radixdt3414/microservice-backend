using buildingBlock.CQRS;
using MediatR;
using Microsoft.Extensions.Logging;
using System.Diagnostics;
using System.Windows.Input;

namespace buildingBlock.Behaviour
{
    public class LoggingBehaviour<TRequste, TResponse>(ILogger<LoggingBehaviour<TRequste, TResponse>> logger) : IPipelineBehavior<TRequste, TResponse>
        where TRequste : notnull, IRequest<TResponse>
        where TResponse : notnull
    {
        public async Task<TResponse> Handle(TRequste request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            logger.LogInformation($"[Start] Request={typeof(TRequste)},  Response={typeof(TResponse)},  RequesteData={request}");
            var sw = new Stopwatch();
            sw.Start();
            var response = await next();
            sw.Stop();
            logger.LogInformation($"[End] Request={typeof(TRequste)},  Response={typeof(TResponse)}, ResponseData={response}");
            logger.LogInformation($"Time taken: {sw.Elapsed}");
            return response;
        }
    }
}