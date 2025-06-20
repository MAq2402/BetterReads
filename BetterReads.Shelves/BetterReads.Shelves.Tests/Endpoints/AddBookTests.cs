using System.Net;
using System.Net.Http.Json;
using BetterReads.Shared.Application.Events;
using BetterReads.Shared.Application.Repositories.Types;
using BetterReads.Shelves.Application.Dtos;
using BetterReads.Shelves.Infra.Mongo.Documents;
using BetterReads.Shelves.Tests.Shared;
using FluentAssertions;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace BetterReads.Shelves.Tests.Endpoints;

public class AddBookTests : TestFactory
{
    [Fact]
    public async Task Should_Work()
    {
        var shelfId = Guid.NewGuid();
        await Repository.CreateShelf(new ShelfDocument {Id = shelfId, Name = "TestShelf", UserId = MockAuthHandler.UserId });
        
        var book = new AddBookDto()
        {
            Name = "Project Hail Mary",
            Author = "Andy Weir",
            Isbn = "9780593135204",
            Language = "English",
            YearOfPublication = 2021,
            ShelfId = shelfId
        };
        
        var response = await Client.PostAsJsonAsync("/shelves/books", book);
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var createdShelf = await Repository.GetShelf("TestShelf");
        createdShelf.Books.Should().HaveCount(1);
        
        var createdBook = createdShelf.Books.First();
        createdBook.Name.Should().Be("Project Hail Mary");
        createdBook.Author.Should().Be("Andy Weir");
        createdBook.Isbn.Should().Be("9780593135204");
        createdBook.Language.Should().Be("English");
        createdBook.YearOfPublication.Should().Be(2021);
        
        var outboxEntry = await Repository.GetOutboxEntry();
        outboxEntry.Should().NotBeNull();
        outboxEntry.Type.Should().Be(typeof(BookAdded).AssemblyQualifiedName);
        outboxEntry.Status.Should().Be(OutboxEventStatus.New);
        outboxEntry.ErrorMessage.Should().BeNullOrEmpty();
        outboxEntry.Event.Should().NotBeNull();

        var @event = JsonSerializer.Deserialize<BookAdded>(outboxEntry.Event);
        @event.Should().NotBeNull();
        @event.UserId.Should().Be(MockAuthHandler.UserId);
    }
    
    [Fact]
    public async Task Should_Fail_If_Shelf_Not_Found()
    {
        var shelfId = Guid.NewGuid();
        
        var book = new AddBookDto()
        {
            Name = "Project Hail Mary",
            Author = "Andy Weir",
            Isbn = "9780593135204",
            Language = "English",
            YearOfPublication = 2021,
            ShelfId = shelfId
        };
        
        var response = await Client.PostAsJsonAsync("/shelves/books", book);
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        
        var outboxEntry = await Repository.GetOutboxEntry();
        outboxEntry.Should().BeNull();
    }
    
    [Fact]
    public async Task Should_Fail_If_Book_Already_Exists()
    {
        var shelfId = Guid.NewGuid();
        await Repository.CreateShelf(new ShelfDocument {Id = shelfId, Name = "TestShelf", UserId = MockAuthHandler.UserId, Books = new List<BookDocument>()
        {
            new()
            {
                Name = "Project Hail Mary",
                Author = "Andy Weir",
                Isbn = "9780593135204",
                Language = "English",
                YearOfPublication = 2021
            }
        }});
        
        var book = new AddBookDto()
        {
            Name = "Project Hail Mary",
            Author = "Andy Weir",
            Isbn = "9780593135204",
            Language = "English",
            YearOfPublication = 2021,
            ShelfId = shelfId
        };
        
        var response = await Client.PostAsJsonAsync("/shelves/books", book);
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        (await response.Content.ReadAsStringAsync()).Should().Be("The book is already on the shelf.");
        
        var outboxEntry = await Repository.GetOutboxEntry();
        outboxEntry.Should().BeNull();
    }
}