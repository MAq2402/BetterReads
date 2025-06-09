namespace BetterReads.Shared.Application.Events;

public record UserRegistered(Guid Id) : IIntegrationEvent;