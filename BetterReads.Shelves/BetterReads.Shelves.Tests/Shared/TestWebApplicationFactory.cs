using BetterReads.Shared.Infra.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MongoDB.Driver;

namespace BetterReads.Shelves.Tests.Shared;

public class TestWebApplicationFactory : WebApplicationFactory<Program>
{
    public HttpClient Client { get; private set; }
    public MongoClient MongoClient { get; private set; } = null!;
    public TestWebApplicationFactory(string connectionString)
    {
        Client = WithWebHostBuilder(builder =>
        {
            builder.ConfigureServices(services =>
            {
                RemoveServices(services);

                MongoClient = new MongoClient(connectionString);
                services.AddSingleton<IMongoClient>(x => MongoClient);
                
                services.AddAuthentication(options =>
                {
                    options.DefaultAuthenticateScheme = MockAuthHandler.SchemeName;
                    options.DefaultChallengeScheme = MockAuthHandler.SchemeName;
                }).AddScheme<AuthenticationSchemeOptions, MockAuthHandler>(
                    MockAuthHandler.SchemeName, options => { });
            });
        }).CreateClient();
    }

    private static void RemoveServices(IServiceCollection services)
    {
        var mongoClientDescriptor = services.SingleOrDefault(x => x.ServiceType == typeof(IMongoClient));

        if (mongoClientDescriptor != null)
        {
            services.Remove(mongoClientDescriptor);
        }
                
        var outboxBackgroundServiceDescriptor = services.SingleOrDefault(
            d => d.ServiceType == typeof(IHostedService) &&
                 d.ImplementationType == typeof(OutboxBackgroundService));

        if (outboxBackgroundServiceDescriptor != null)
        {
            services.Remove(outboxBackgroundServiceDescriptor);
        }
    }
}