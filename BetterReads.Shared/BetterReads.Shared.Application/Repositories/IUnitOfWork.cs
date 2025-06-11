using BetterReads.Shared.Application.Repositories.Types;

namespace BetterReads.Shared.Application.Repositories;

public interface IUnitOfWork
{
    Task<IDbSession> StartSession();
    Task Transaction(Func<IDbSession, Task> action);
}