using BetterReads.Shared.Infra.Documents;

namespace BetterReads.Shared.Infra.Repositories;

public interface IMongoRepository<TDocument, in TId> where TDocument : IMongoDocument<TId>
    where TId : IEquatable<TId>
{
    Task Add(TDocument document);
    Task<TDocument?> Get(TId id);
    Task<IEnumerable<TDocument>> GetAll();
    Task Save(TDocument document);
}