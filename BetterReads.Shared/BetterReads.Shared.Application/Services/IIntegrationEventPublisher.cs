using BetterReads.Shared.Application.Events;

namespace BetterReads.Shared.Application.Services;

public interface IIntegrationEventPublisher
{
    Task Publish<T>(T @event) where T : IIntegrationEvent;
}