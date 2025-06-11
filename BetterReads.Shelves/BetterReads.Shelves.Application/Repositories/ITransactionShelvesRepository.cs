using BetterReads.Shared.Application.Repositories.Types;
using BetterReads.Shelves.Domain;
using BetterReads.Shelves.Domain.Repositories;

namespace BetterReads.Shelves.Application.Repositories;

public interface ITransactionShelvesRepository : IShelvesRepository
{
    Task Save(Shelf shelf, IDbSession dbSession);
}