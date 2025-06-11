using System.Text.Json;
using BetterReads.Shared.Application.Events;
using BetterReads.Shared.Application.Repositories;
using BetterReads.Shared.Application.Repositories.Types;
using BetterReads.Shared.Infra.Documents;
using BetterReads.Shared.Infra.Repositories.Types;

namespace BetterReads.Shared.Infra.Repositories;

public class MongoOutboxRepository(IMongoRepository<OutboxDocument, Guid> repository) : IOutboxRepository
{
    public async Task Add(OutboxDocument outbox)
    {
        await repository.Add(outbox);
    }

    public async Task Add<T>(T integrationEvent) where T : IIntegrationEvent
    {
        await repository.Add(new OutboxDocument()
        {
            Event = JsonSerializer.Serialize(integrationEvent),
            Type = typeof(T).FullName,
            Status = OutboxEventStatus.New
        });
    }

    public async Task Add<T>(T integrationEvent, IDbSession dbSession) where T : IIntegrationEvent
    {
        var mongoDbSession = dbSession as MongoDbSession;

        if (mongoDbSession is null)
        {
            throw new ArgumentException("DbSession is not a MongoDbSession");
        } 
        
        await repository.Add(new OutboxDocument()
        {
            Event = JsonSerializer.Serialize(integrationEvent),
            Type = typeof(T).FullName,
            Status = OutboxEventStatus.New
        }, mongoDbSession.Session);
    }
}