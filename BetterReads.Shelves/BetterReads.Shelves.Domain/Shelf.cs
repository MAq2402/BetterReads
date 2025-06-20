using BetterReads.Shared.Domain.Base;
using BetterReads.Shelves.Domain.Events;
using BetterReads.Shelves.Domain.Exceptions;

namespace BetterReads.Shelves.Domain;

public class Shelf : AggregateRoot
{
    private readonly List<Book> _books = new();
    public Shelf(AggregateId id, string name, Guid userId) : base(id)
    {
        Name = name;
        UserId = userId;
    }
    
    public Shelf(AggregateId id, int version, string name, Guid userId, List<Book>? books) : base(id)
    {
        Name = name;
        UserId = userId;
        _books = books ?? new List<Book>();
        Version = version;
    }

    public string Name { get; private set; }
    public Guid UserId { get; private set; }
    public List<Book> Books => _books;

    public static Shelf WantToRead(Guid userId)
    {
        return new Shelf(Guid.NewGuid(), "Want to Read", userId);
    }
    
    public static Shelf Read(Guid userId)
    {
        return new Shelf(Guid.NewGuid(), "Read", userId);
    }
    
    public static Shelf CurrentlyReading(Guid userId)
    {
        return new Shelf(Guid.NewGuid(), "Currently Reading", userId);
    }
    
    public void AddBook(Book book)
    {
        if (_books.Any(x => x == book))
        {
            throw new AddingBookWithExistingIsbnException("The book is already on the shelf.");
        }
        
        _books.Add(book);
        AddEvent(new BookAdded());
    }
}