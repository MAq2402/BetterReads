using BetterReads.Shared.Infra.Documents;

namespace BetterReads.Shelves.Infra.Mongo.Documents;

public class ShelfDocument : IMongoDocument<Guid>
{
    public static string CollectionName() => "Shelves";
    public Guid Id { get; set; }
    public int Version { get; set; }
    public Guid UserId { get; set; }
    public string Name { get; set; } = string.Empty;
    public List<BookDocument>? Books { get; set; }
}