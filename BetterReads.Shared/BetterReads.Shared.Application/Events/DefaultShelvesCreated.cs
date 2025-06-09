namespace BetterReads.Shared.Application.Events;

public record DefaultShelvesCreated(Guid UserId) : IIntegrationEvent;