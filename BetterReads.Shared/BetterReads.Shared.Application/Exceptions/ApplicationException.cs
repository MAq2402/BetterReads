using System.Net;

namespace BetterReads.Shared.Application.Exceptions;

public abstract class ApplicationException(string message, HttpStatusCode statusCode) : Exception(message)
{
    public HttpStatusCode StatusCode { get; private set; } = statusCode;
}