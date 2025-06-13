using BetterReads.Shared.Application.Events;
using BetterReads.Shared.Application.Repositories.Types;

namespace BetterReads.Shared.Application.Repositories;

public interface IOutboxRepository
{
    Task Add<T>(T integrationEvent, IDbSession dbSession) where T : IIntegrationEvent;
    Task<List<OutboxModel>> GetUnprocessedEvents();
    Task MarkAsProcessed(Guid id);
    Task MarkAsFailedToDeliver(Guid id, string errorMessage);
}