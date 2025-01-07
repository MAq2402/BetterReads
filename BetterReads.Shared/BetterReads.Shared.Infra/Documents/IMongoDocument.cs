namespace BetterReads.Shared.Infra.Documents;

public interface IMongoDocument<TId> where TId : IEquatable<TId>
{
    static abstract string CollectionName();
    public TId Id { get; set; }
    public int Version { get; set; }
}