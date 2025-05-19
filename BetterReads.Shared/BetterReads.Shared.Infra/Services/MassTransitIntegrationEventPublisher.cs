using BetterReads.Shared.Application.Events;
using BetterReads.Shared.Application.Services;
using MassTransit;

namespace BetterReads.Shared.Infra.Services;

public class MassTransitIntegrationEventPublisher(IPublishEndpoint publishEndpoint) : IIntegrationEventPublisher
{
    public async Task Publish<T>(T @event) where T : IIntegrationEvent
    {
        await publishEndpoint.Publish(@event);
    }
}