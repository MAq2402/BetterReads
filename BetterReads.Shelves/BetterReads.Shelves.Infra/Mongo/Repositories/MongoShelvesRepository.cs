using BetterReads.Shared.Domain.Base;
using BetterReads.Shared.Infra.Repositories;
using BetterReads.Shelves.Domain;
using BetterReads.Shelves.Domain.Repositories;
using BetterReads.Shelves.Infra.Mongo.Documents;
using BetterReads.Shelves.Infra.Mongo.Mappings;
using MongoDB.Driver;

namespace BetterReads.Shelves.Infra.Mongo.Repositories;

public class MongoShelvesRepository(IMongoRepository<ShelfDocument, Guid> repository) : IShelvesRepository
{
    public async Task AddShelf(Shelf shelf)
    {
        await repository.Add(shelf.AsDocument());
    }

    public async Task<Shelf?> Get(AggregateId id, Guid userId)
    {
        var shelf = await repository.Get(id);

        if (shelf == null)
        {
            return null;
        }
        
        if (shelf.UserId != userId)
        {
            return null;
        }

        return (await repository.Get(id))?.AsEntity() ?? null;
    }

    public async Task<List<Shelf>> GetMany(Guid userId)
    {
        return (await repository.GetMany(Builders<ShelfDocument>.Filter.Eq("UserId", userId))).Select(x => x.AsEntity()).ToList();
    }

    public async Task Save(Shelf shelf)
    {
        await repository.Save(shelf.AsDocument());
    }
}