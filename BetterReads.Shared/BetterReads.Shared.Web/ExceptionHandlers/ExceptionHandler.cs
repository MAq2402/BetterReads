using BetterReads.Shared.Domain.Exceptions;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;

namespace BetterReads.Shared.Web.ExceptionHandlers;

public class ExceptionHandler() : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
    {
        switch (exception)
        {
            case Application.Exceptions.ApplicationException applicationException:
                httpContext.Response.StatusCode = (int)applicationException.StatusCode;
                await httpContext.Response.WriteAsync(applicationException.Message);
                return true;
            case DomainException domainException:
                httpContext.Response.StatusCode = StatusCodes.Status400BadRequest;
                await httpContext.Response.WriteAsync(domainException.Message);
                return true;
            case not null:
                httpContext.Response.StatusCode = StatusCodes.Status500InternalServerError;
                await httpContext.Response.WriteAsync("Unhandled exception occured.");
                return true;
        }
        
        return false;
    }
}