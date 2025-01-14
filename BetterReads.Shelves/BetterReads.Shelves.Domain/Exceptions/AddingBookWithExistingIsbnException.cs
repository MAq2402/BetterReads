using BetterReads.Shared.Domain.Exceptions;

namespace BetterReads.Shelves.Domain.Exceptions;

public class AddingBookWithExistingIsbnException(string message) : DomainException(message)
{
}