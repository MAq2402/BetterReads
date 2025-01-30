using BetterReads.Recommendations.Application.Models;

namespace BetterReads.Recommendations.Application.Services;

public interface IShelvesService
{
    Task<List<ShelfDto>> GetShelves(Guid userId);
}