using MediatR;
using Microsoft.Extensions.Logging;
using System.Diagnostics;

namespace Transit.Application.Common.Behaviors;

public class LoggingBehavior<TRequest, TResponse>(ILogger<LoggingBehavior<TRequest, TResponse>> logger)
    : IPipelineBehavior<TRequest, TResponse>
    where TRequest : notnull
{
    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken ct)
    {
        var requestName = typeof(TRequest).Name;
        logger.LogInformation("Handling {Request}", requestName);
        var sw = Stopwatch.StartNew();
        var response = await next(ct);
        sw.Stop();
        logger.LogInformation("Handled {Request} in {ElapsedMs}ms", requestName, sw.ElapsedMilliseconds);
        return response;
    }
}
