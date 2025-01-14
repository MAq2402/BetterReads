using BetterReads.Shared.Domain.Exceptions;

namespace BetterReads.Shelves.Domain.Exceptions;

public class ShelfWithNameAlreadyExistsException(string message) : DomainException(message)
{
}