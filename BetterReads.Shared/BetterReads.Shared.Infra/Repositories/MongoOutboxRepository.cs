using System.Text.Json;
using BetterReads.Shared.Application.Events;
using BetterReads.Shared.Application.Repositories;
using BetterReads.Shared.Application.Repositories.Types;
using BetterReads.Shared.Infra.Documents;
using BetterReads.Shared.Infra.Repositories.Types;
using MongoDB.Driver;

namespace BetterReads.Shared.Infra.Repositories;

public class MongoOutboxRepository(IMongoRepository<OutboxDocument, Guid> repository) : IOutboxRepository
{
    private const int EventsFetchLimit = 100;

    public async Task Add<T>(T integrationEvent, IDbSession dbSession) where T : IIntegrationEvent
    {
        var mongoDbSession = dbSession as MongoDbSession;

        if (mongoDbSession is null)
        {
            throw new ArgumentException("DbSession is not a MongoDbSession");
        }

        await repository.Add(new OutboxDocument
        {
            Event = JsonSerializer.Serialize(integrationEvent),
            Type = typeof(T).AssemblyQualifiedName,
            Status = OutboxEventStatus.New
        }, mongoDbSession.Session);
    }

    public async Task<List<OutboxModel>> GetUnprocessedEvents()
    {
        return (await repository.GetMany(Builders<OutboxDocument>.Filter.Where(x => x.Status == OutboxEventStatus.New),
                EventsFetchLimit))
            .Select(x => new OutboxModel
            {
                ErrorMessage = x.ErrorMessage,
                Event = x.Event ?? string.Empty,
                Type = x.Type ?? string.Empty,
                Status = x.Status,
                Version = x.Version,
                Id = x.Id
            }).ToList();
    }

    public async Task MarkAsProcessed(Guid id)
    {
        await repository.GetCollection().UpdateOneAsync(x => x.Id == id,
            Builders<OutboxDocument>.Update.Set(m => m.Status, OutboxEventStatus.Delivered));
    }

    public async Task MarkAsFailedToDeliver(Guid id, string errorMessage)
    {
        await repository.GetCollection().UpdateOneAsync(x => x.Id == id,
            Builders<OutboxDocument>.Update.Set(m => m.Status, OutboxEventStatus.FailedToDeliver)
                .Set(m => m.ErrorMessage, errorMessage));
    }
}