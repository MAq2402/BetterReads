using BetterReads.Shared.Infra.Repositories;
using BetterReads.Shelves.Domain;
using BetterReads.Shelves.Domain.Repositories;
using BetterReads.Shelves.Infra.Mongo.Documents;
using BetterReads.Shelves.Infra.Mongo.Mappings;

namespace BetterReads.Shelves.Infra.Mongo.Repositories;

public class MongoShelvesRepository(IMongoRepository<ShelfDocument, Guid> repository) : IShelvesRepository
{
    public async Task AddShelf(Shelf shelf)
    {
        await repository.Add(shelf.AsDocument());
    }

    public async Task<Shelf?> Get(Guid id, Guid userId)
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

    public async Task<List<Shelf>> GetAll(Guid userId)
    {
        return (await repository.GetAll()).Where(x => x.UserId == userId).Select(x => x.AsEntity()).ToList();
    }

    public async Task Save(Shelf shelf)
    {
        await repository.Save(shelf.AsDocument());
    }
}