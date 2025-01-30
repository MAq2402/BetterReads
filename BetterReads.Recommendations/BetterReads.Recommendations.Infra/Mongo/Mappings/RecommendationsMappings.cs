using BetterReads.Recommendations.Infra.Mongo.Documents;

namespace BetterReads.Recommendations.Infra.Mongo.Mappings;

public static class RecommendationsMappings
{
    public static RecommendationsDocument AsDocument(this Domain.Entities.Recommendations recommendations)
    {
        return new RecommendationsDocument()
        {
            Id = recommendations.Id,
            Version = recommendations.Version,
            UserId = recommendations.UserId,
            Books = recommendations.Books.ToList()
        };
    }

    public static Domain.Entities.Recommendations? AsEntity(this RecommendationsDocument? recommendations)
    {
        if (recommendations == null) return null;
        return new Domain.Entities.Recommendations(recommendations.Id, recommendations.UserId,
            recommendations.Books.ToList(), recommendations.Version);
    }
}