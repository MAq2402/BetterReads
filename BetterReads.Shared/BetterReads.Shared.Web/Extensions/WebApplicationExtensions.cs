using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;

namespace BetterReads.Shared.Web.Extensions;

public static class WebApplicationExtensions
{
    public static RouteHandlerBuilder MediatorMapPost<TRequest>(this WebApplication app, string pattern) where TRequest : IRequest
    {
        return app.MapPost(pattern, async (IMediator mediator, TRequest request) => await mediator.Send(request))
            .WithOpenApi();
    }

    public static RouteHandlerBuilder MediatorMapPut<TRequest>(this WebApplication app, string pattern)
        where TRequest : IRequest
    {
        return app.MapPut(pattern, async (IMediator mediator, TRequest request) => await mediator.Send(request))
            .WithOpenApi();
    }

    public static RouteHandlerBuilder MediatorMapGet<TRequest, TResponse>(this WebApplication app, string pattern, TRequest request) where TRequest : IRequest<TResponse>
    {
        return app.MapGet(pattern, async (IMediator mediator) => await mediator.Send(request))
            .WithOpenApi();
    }
    
    public static RouteHandlerBuilder MediatorMapGet<TRequest, TResponse>(this WebApplication app, string pattern) where TRequest : IRequest<TResponse>
    {
        return app.MapGet(pattern, async (IMediator mediator, [AsParameters] TRequest request) => await mediator.Send(request))
            .WithOpenApi();
    }
}