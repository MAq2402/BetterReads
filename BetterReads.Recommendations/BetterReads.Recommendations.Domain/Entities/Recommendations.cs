using BetterReads.Recommendations.Domain.Events;
using BetterReads.Recommendations.Domain.ValueObjects;
using BetterReads.Shared.Domain.Base;

namespace BetterReads.Recommendations.Domain.Entities;

public class Recommendations : AggregateRoot
{
    private List<Book> _books;
    public Recommendations(AggregateId id, Guid userId, List<Book> books) : base(id)
    {
        _books = books;
        UserId = userId;
    }
    
    public Recommendations(AggregateId id, Guid userId, List<Book> books, int version) : base(id)
    {
        Version = version;
        _books = books;
        UserId = userId;
    }

    public void UpdateBooks(List<Book> books)
    {
        _books = books;
        AddEvent(new BooksUpdated());
    }

    public Guid UserId { get; private set; }
    public List<Book> Books => _books;
}