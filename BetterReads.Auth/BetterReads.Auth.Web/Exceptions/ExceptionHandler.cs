using System.Net;
using BetterReads.Shared.Application.Exceptions;
using Microsoft.AspNetCore.Diagnostics;

namespace BetterReads.Auth.Web.Exceptions;

public class ExceptionHandler : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
    {
        switch (exception)
        {
            case UnauthorizedException e:
                await httpContext.Response.WriteAsync(e.Message, cancellationToken: cancellationToken);
                httpContext.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                break;
            default:
                await httpContext.Response.WriteAsync("Something went wrong, try again later", cancellationToken: cancellationToken);
                httpContext.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                break;
        }
        return true;
    }
}