namespace BetterReads.Shared.Application.Exceptions;

public class UnauthorizedException(string message) : ApplicationException(message)
{
}