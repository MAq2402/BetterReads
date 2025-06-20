using BetterReads.Shelves.Infra.Mongo.Documents;
using MongoDB.Driver;

namespace BetterReads.Shelves.Tests.Shared;

public class Repository
{
    public IMongoCollection<ShelfDocument> Shelves { get; private set; }
    
    public Repository(IMongoClient mongoClient)
    {
        var mongoDatabase = mongoClient.GetDatabase("betterReads_shelves");
        Shelves = mongoDatabase.GetCollection<ShelfDocument>("Shelves");
    }
    
    public async Task CreateShelf(ShelfDocument shelf)
    {
        await Shelves.InsertOneAsync(shelf);
    }
}