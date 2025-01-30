using BetterReads.Shared.Domain.Base;

namespace BetterReads.Shelves.Domain.Repositories;

public interface IShelvesRepository
{
    Task AddShelf(Shelf shelf);
    Task<Shelf?> Get(AggregateId id, Guid userId);
    Task<List<Shelf>> GetMany(Guid userId);
    Task Save(Shelf shelf);
}