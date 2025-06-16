using System.Net;
using System.Net.Http.Json;
using BetterReads.Shelves.Application.Dtos;
using BetterReads.Shelves.Infra.Mongo.Documents;
using BetterReads.Shelves.Tests.Shared;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;

namespace BetterReads.Shelves.Tests.Endpoints;

[CollectionDefinition("Shelves")]
public class CreateShelfEndpointTests : IClassFixture<TestWebApplicationFactory>
{
    private readonly TestWebApplicationFactory _factory;

    public CreateShelfEndpointTests(TestWebApplicationFactory factory)
    {
        _factory = factory;
    }
    
    [Fact]
    public async Task Should_Create_Shelf()
    {
        // var factory = new TestWebApplicationFactory();
        var _client = _factory.Client;
        var book = new CreateShelfDto { Name = "TestShelf" };
        
        var response = await _client.PostAsJsonAsync("/shelves", book);
        
        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }
    
    [Fact]
    public async Task Should_Return_BadRequest_Shelf_Already_Exists()
    {
        // var factory = new TestWebApplicationFactory();
        var _client = _factory.Client;
        // var mongo = _factory.Services.GetRequiredService<IMongoClient>();
        // var db = mongo.GetDatabase("betterReads_shelves");
        // var collection = db.GetCollection<ShelfDocument>("Shelves");
        //
        // await collection.InsertOneAsync(new ShelfDocument { Name = "TestShelf", UserId = MockAuthHandler.UserId });
        
        var book = new CreateShelfDto { Name = "TestShelf" };
        
        var response = await _client.PostAsJsonAsync("/shelves", book);
        
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        (await response.Content.ReadAsStringAsync()).Should().Be("Shelf with name already exists");
    }
}