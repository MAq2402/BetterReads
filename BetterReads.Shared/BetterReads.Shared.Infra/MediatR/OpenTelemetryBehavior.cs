using System.Diagnostics;
using MediatR;

namespace BetterReads.Shared.Infra.MediatR;

public class OpenTelemetryBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse> where TRequest : notnull
{
    private static readonly ActivitySource ActivitySource = new("MediatR");

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        using var activity = ActivitySource.StartActivity(typeof(TRequest).Name);

        return await next();
    }
}