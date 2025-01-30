using BetterReads.Recommendations.Domain.Repositories;
using BetterReads.Recommendations.Infra.Mongo.Documents;
using BetterReads.Recommendations.Infra.Mongo.Mappings;
using BetterReads.Shared.Infra.Repositories;
using MongoDB.Driver;

namespace BetterReads.Recommendations.Infra.Mongo.Repositories;

public class MongoRecommendationsRepository(IMongoRepository<RecommendationsDocument, Guid> repository) : IRecommendationsRepository
{
    public async Task<Domain.Entities.Recommendations?> Get(Guid userId)
    {
        return (await repository.Get(Builders<RecommendationsDocument>.Filter.Eq("UserId", userId)))?.AsEntity();
    }

    public async Task Save(Domain.Entities.Recommendations recommendations)
    {
        await repository.Save(recommendations.AsDocument());
    }

    public async Task Add(Domain.Entities.Recommendations recommendations)
    {
        await repository.Add(recommendations.AsDocument());
    }
}