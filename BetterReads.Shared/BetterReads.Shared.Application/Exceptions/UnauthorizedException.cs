using System.Net;

namespace BetterReads.Shared.Application.Exceptions;

public class UnauthorizedException(string message) : ApplicationException(message, HttpStatusCode.Unauthorized)
{
}