using DotNet.Testcontainers.Builders;
using Testcontainers.MongoDb;

namespace BetterReads.Shelves.Tests.Shared;

public class TestFactory : IAsyncLifetime
{
    private const int MongoPort = 27017;
    private TestWebApplicationFactory _factory = null!;
    private MongoDbContainer _mongoDbContainer  = null!;
    protected HttpClient Client => _factory.Client;
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
            .WithPortBinding(MongoPort, true)
            .WithCommand("mongod", "--replSet", "rs0", "--bind_ip_all")
            .WithWaitStrategy(Wait.ForUnixContainer().UntilPortIsAvailable(MongoPort))
            .WithCleanUp(true)
            .Build();
        await _mongoDbContainer.StartAsync();
        await _mongoDbContainer.ExecScriptAsync("rs.initiate();");
    }

    public async Task DisposeAsync()
    {
        await _factory.DisposeAsync();
        await _mongoDbContainer.DisposeAsync();
    }
}