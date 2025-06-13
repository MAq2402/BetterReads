using BetterReads.Shared.Application.Repositories.Types;
using MongoDB.Driver;

namespace BetterReads.Shared.Infra.Repositories.Types;

public class MongoDbSession : IDbSession
{
    public MongoDbSession(IClientSessionHandle session)
    {
        Session = session;
    }

    public IClientSessionHandle Session { get; private set; }
    
    public async Task CommitTransaction()
    {
        await Session.CommitTransactionAsync();
    }

    public async Task AbortTransaction()
    {
        await Session.AbortTransactionAsync();
    }

    public void StartTransaction()
    {
        Session.StartTransaction();
    }

    public void Dispose()
    {
        Session.Dispose();
    }
}