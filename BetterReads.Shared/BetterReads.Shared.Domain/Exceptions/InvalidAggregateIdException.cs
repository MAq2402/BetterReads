namespace BetterReads.Shared.Domain.Exceptions;

public class InvalidAggregateIdException : DomainException
{
    public Guid Id { get; }
    public InvalidAggregateIdException(Guid id) : base($"Invalid aggregate id: {id}")
    {
        Id = id;
    }
}