namespace BetterReads.Shared.Application.Repositories.Types;

public interface IDbSession : IDisposable
{
    Task CommitTransaction();
    Task AbortTransaction();
    void StartTransaction();
}
