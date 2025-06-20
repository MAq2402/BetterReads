using BetterReads.Shared.Infra.Documents;
using BetterReads.Shelves.Infra.Mongo.Documents;
using MongoDB.Driver;

namespace BetterReads.Shelves.Tests.Shared;

public class Repository
{
    public IMongoCollection<ShelfDocument> Shelves { get; private set; }
    public IMongoCollection<OutboxDocument> OutboxCollection { get; private set; }
    
    public Repository(IMongoClient mongoClient)
    {
        var mongoDatabase = mongoClient.GetDatabase("betterReads_shelves");
        Shelves = mongoDatabase.GetCollection<ShelfDocument>("Shelves");
        OutboxCollection = mongoDatabase.GetCollection<OutboxDocument>("Outbox");
    }
    
    public async Task CreateShelf(ShelfDocument shelf)
    {
        await Shelves.InsertOneAsync(shelf);
    }

    public async Task<ShelfDocument> GetShelf(string shelfName)
    {
        return await Shelves.Find(x => x.Name == shelfName).SingleOrDefaultAsync();
    }
    
    public async Task<OutboxDocument> GetOutboxEntry()
    {
        return await OutboxCollection.Find(FilterDefinition<OutboxDocument>.Empty).SingleOrDefaultAsync();
    }
    
    public async Task<bool> NoShelves()
    {
        return await Shelves.CountDocumentsAsync(FilterDefinition<ShelfDocument>.Empty) == 0;
    }
    
    public async Task<bool> NoOutboxEntries()
    {
        return await OutboxCollection.CountDocumentsAsync(FilterDefinition<OutboxDocument>.Empty) == 0;
    }
}