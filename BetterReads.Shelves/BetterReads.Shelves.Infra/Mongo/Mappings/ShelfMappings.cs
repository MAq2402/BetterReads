using BetterReads.Shelves.Domain;
using BetterReads.Shelves.Infra.Mongo.Documents;

namespace BetterReads.Shelves.Infra.Mongo.Mappings;

public static class ShelfMappings
{
    public static ShelfDocument AsDocument(this Shelf shelf)
    {
        return new ShelfDocument
        {
            Id = shelf.Id,
            Version = shelf.Version,
            Name = shelf.Name,
            UserId = shelf.UserId,
            Books = shelf.Books.Select(x => new BookDocument
            {
                Name = x.Name,
                Isbn = x.Isbn,
                Author = x.Author,
                YearOfPublication = x.YearOfPublication,
                Language = x.Language
            }).ToList()
        };
    }

    public static Shelf AsEntity(this ShelfDocument shelf)
    {
        return new Shelf(shelf.Id, shelf.Version, shelf.Name, shelf.UserId,
            shelf.Books.Select(x => new Book(x.Name, x.Author, x.Isbn, x.Language, x.YearOfPublication)).ToList());
    }
}