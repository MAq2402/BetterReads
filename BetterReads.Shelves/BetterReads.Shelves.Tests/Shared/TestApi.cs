using Testcontainers.MongoDb;

namespace BetterReads.Shelves.Tests.Shared;

public class TestApi : IAsyncLifetime
{
    public HttpClient HttpClient { get; private set; }
    public MongoDbContainer MongoDbContainer { get; private set; }
    
    public Task InitializeAsync()
    {
        throw new NotImplementedException();
    }

    public Task DisposeAsync()
    {
        throw new NotImplementedException();
    }
}