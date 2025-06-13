using BetterReads.Shared.Application.Repositories;
using BetterReads.Shared.Application.Repositories.Types;
using BetterReads.Shared.Infra.Repositories.Types;
using MongoDB.Driver;

namespace BetterReads.Shared.Infra.Repositories;

public class MongoUnitOfWork(IMongoClient client) : IUnitOfWork
{
    public async Task<IDbSession> StartSession()
    {
        return new MongoDbSession(await client.StartSessionAsync());
    }

    public async Task Transaction(Func<IDbSession, Task> action)
    {
        using var session = await client.StartSessionAsync();
        session.StartTransaction();

        try
        {
            await action(new MongoDbSession(session));
            await session.CommitTransactionAsync();
        }
        catch
        {
            await session.AbortTransactionAsync();
            throw;
        }
    }
}