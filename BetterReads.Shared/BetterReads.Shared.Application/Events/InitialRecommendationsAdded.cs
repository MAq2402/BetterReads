namespace BetterReads.Shared.Application.Events;

public record InitialRecommendationsAdded(Guid UserId) : IIntegrationEvent;