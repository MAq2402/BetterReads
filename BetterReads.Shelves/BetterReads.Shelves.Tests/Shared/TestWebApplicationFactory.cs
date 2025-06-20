using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;

namespace BetterReads.Shelves.Tests.Shared;

public class TestWebApplicationFactory : WebApplicationFactory<Program>
{
    public HttpClient Client { get; private set; }
    public MongoClient? MongoClient { get; private set; }
    public TestWebApplicationFactory(string connectionString)
    {
        Client = WithWebHostBuilder(builder =>
        {
            builder.ConfigureServices(services =>
            {
                var descriptor = services.SingleOrDefault(x => x.ServiceType == typeof(IMongoClient));

                if (descriptor != null)
                {
                    services.Remove(descriptor);
                }

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
}