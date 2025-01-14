using BetterReads.Shared.Infra.Documents;
using BetterReads.Shared.Infra.Settings;
using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;
using MongoDB.Driver;

namespace BetterReads.Shared.Infra.Repositories;

internal sealed class MongoRepository<TDocument, TId> : IMongoRepository<TDocument, TId> where TDocument : IMongoDocument<TId>  
    where TId : IEquatable<TId>
{
    private readonly IMongoCollection<TDocument> _collection;

    public MongoRepository(IOptions<MongoSettings> mongoSettings)
    {
        BsonSerializer.RegisterSerializer(new GuidSerializer(GuidRepresentation.Standard));
        var client = new MongoClient(mongoSettings.Value.ConnectionString);
        var database = client.GetDatabase(mongoSettings.Value.DatabaseName);
        _collection = database.GetCollection<TDocument>(TDocument.CollectionName());
    }

    public async Task Add(TDocument document)
    {
        await _collection.InsertOneAsync(document);
    }

    public async Task<TDocument?> Get(TId id)
    {
        return (await _collection.FindAsync(x => x.Id.Equals(id))).FirstOrDefault();
    }

    public async Task<IEnumerable<TDocument>> GetAll()
    {
        return (await _collection.FindAsync(_ => true)).ToEnumerable();
    }

    public async Task Save(TDocument document)
    {
        await _collection.ReplaceOneAsync(x => x.Id.Equals(document.Id) && x.Version < document.Version, document);
    }
}