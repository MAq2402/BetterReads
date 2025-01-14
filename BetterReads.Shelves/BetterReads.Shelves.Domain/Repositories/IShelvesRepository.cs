namespace BetterReads.Shelves.Domain.Repositories;

public interface IShelvesRepository
{
    Task AddShelf(Shelf shelf);
    Task<Shelf?> Get(Guid id, Guid userId);
    Task<List<Shelf>> GetAll(Guid userId);
    Task Save(Shelf shelf);
}