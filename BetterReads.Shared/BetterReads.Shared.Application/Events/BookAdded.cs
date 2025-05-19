namespace BetterReads.Shared.Application.Events;

public record BookAdded(Guid UserId) : IIntegrationEvent;