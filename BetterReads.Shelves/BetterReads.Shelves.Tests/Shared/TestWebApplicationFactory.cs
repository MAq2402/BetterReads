using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;
using Testcontainers.MongoDb;

namespace BetterReads.Shelves.Tests.Shared;

public class TestWebApplicationFactory : WebApplicationFactory<Program>, IAsyncLifetime
{
    private MongoDbContainer  _mongoContainer;
    public HttpClient Client { get; set; }
    // public string MongoConnectionString => _mongoContainer.GetConnectionString();

    public TestWebApplicationFactory()
    {

    }
    
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        //CONFIGURE TEST SERVICES METHOD INSTEAD?
        builder.ConfigureTestServices(services =>
        {
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = MockAuthHandler.SchemeName;
                options.DefaultChallengeScheme = MockAuthHandler.SchemeName;
            }).AddScheme<AuthenticationSchemeOptions, MockAuthHandler>(
                MockAuthHandler.SchemeName, options => { });
        });
    }

    public async Task InitializeAsync()
    {
        _mongoContainer = new MongoDbBuilder()
            .WithPassword(string.Empty)
            .WithUsername(string.Empty)
            .WithImage("mongo:7.0")
            .WithPortBinding(27017, true)
            .WithExposedPort(27017)
            // .WithExtraHost("host.docker.internal:host-gateway", "27017")
            // .WithReplicaSet()
            .WithCleanUp(true)
            .Build();
        await _mongoContainer.StartAsync();
        Client = WithWebHostBuilder(x =>
        {
            x.ConfigureServices(services =>
            {
                var descriptor = services.SingleOrDefault(x => x.ServiceType == typeof(IMongoClient));

                if (descriptor != null)
                {
                    services.Remove(descriptor);
                }

                services.AddSingleton<IMongoClient>(x => new MongoClient(_mongoContainer.GetConnectionString()));
            });
        }).CreateClient();
    }

    public new async Task DisposeAsync()
    { 
        await _mongoContainer.StopAsync();
    }
}