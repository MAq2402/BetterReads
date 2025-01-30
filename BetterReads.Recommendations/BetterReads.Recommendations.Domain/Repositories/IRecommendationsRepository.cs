namespace BetterReads.Recommendations.Domain.Repositories;

public interface IRecommendationsRepository
{
    Task<Entities.Recommendations?> Get(Guid userId);
    Task Save(Entities.Recommendations recommendations);
    Task Add(Entities.Recommendations recommendations);
}