using BetterReads.Recommendations.Domain.ValueObjects;
using BetterReads.Shared.Infra.Documents;

namespace BetterReads.Recommendations.Infra.Mongo.Documents;

public class RecommendationsDocument : IMongoDocument<Guid>
{
    public static string CollectionName() => "Recommendations";
    public Guid Id { get; set; }
    public int Version { get; set; }
    public Guid UserId { get; set; }
    public List<Book> Books { get; set; }
}