using System.Net;
using BetterReads.Shared.Domain.Base;

namespace BetterReads.Shared.Application.Exceptions;

public class AggregateNotFoundException : ApplicationException
{
    public AggregateNotFoundException(string name, AggregateId id) : base($"Could not find {name} with id: {id}", HttpStatusCode.NotFound)
    {
        Name = name;
    }

    private string Name { get; }
}