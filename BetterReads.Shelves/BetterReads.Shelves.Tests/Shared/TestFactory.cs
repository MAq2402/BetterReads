using Testcontainers.MongoDb;

namespace BetterReads.Shelves.Tests.Shared;

public class TestFactory : IAsyncLifetime
{
    private TestWebApplicationFactory? _factory;
    private MongoDbContainer? _mongoDbContainer;
    protected HttpClient? Client => _factory?.Client;
    protected Repository Repository { get; private set; } = null!;
    
    public async Task InitializeAsync()
    {
        await InitMongo();
        _factory = new TestWebApplicationFactory(_mongoDbContainer!.GetConnectionString());
        Repository = new Repository(_factory.MongoClient!);
    }

    private async Task InitMongo()
    {
        _mongoDbContainer = new MongoDbBuilder()
            .WithPassword(string.Empty)
            .WithUsername(string.Empty)
            .WithImage("mongo:7.0")
            .WithPortBinding(27017, true)
            .WithExposedPort(27017)
            // .WithExtraHost("host.docker.internal:host-gateway", "27017")
            // .WithReplicaSet()
            .WithCleanUp(true)
            .Build();
        await _mongoDbContainer.StartAsync();
    }

    public async Task DisposeAsync()
    {
        if (_factory != null)
        {
            await _factory.DisposeAsync();
        }

        if (_mongoDbContainer != null)
        {
            await _mongoDbContainer.DisposeAsync();
        }
    }
}