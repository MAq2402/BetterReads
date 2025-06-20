using System.Net;
using System.Net.Http.Json;
using BetterReads.Shelves.Application.Dtos;
using BetterReads.Shelves.Infra.Mongo.Documents;
using BetterReads.Shelves.Tests.Shared;
using FluentAssertions;

namespace BetterReads.Shelves.Tests.Endpoints;
public class CreateShelfEndpointTestsFactory : TestFactory
{
    [Fact]
    public async Task Should_Create_Shelf()
    {
        var book = new CreateShelfDto { Name = "TestShelf" };
        
        var response = await Client.PostAsJsonAsync("/shelves", book);
        
        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }
    
    [Fact]
    public async Task Should_Return_BadRequest_Shelf_Already_Exists()
    {
        await Repository.CreateShelf(new ShelfDocument { Name = "TestShelf", UserId = MockAuthHandler.UserId });
        
        var book = new CreateShelfDto { Name = "TestShelf" };
        
        var response = await Client.PostAsJsonAsync("/shelves", book);
        
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        (await response.Content.ReadAsStringAsync()).Should().Be("Shelf with name already exists");
    }
}