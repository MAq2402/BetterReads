using BetterReads.Shelves.Application.Dtos;
using BetterReads.Shelves.Domain;

namespace BetterReads.Shelves.Application.Mappings;

public static class ShelfMappings
{
    public static ShelfDto AsDto(this Shelf shelf)
    {
        return new ShelfDto
        {
            Id = shelf.Id,
            Name = shelf.Name,
            Books = shelf.Books.Select(AsDto).ToList(),
            UserId = shelf.UserId,
        };
    }

    public static BookDto AsDto(this Book book)
    {
        return new BookDto
        {
            Author = book.Author,
            Isbn = book.Isbn,
            YearOfPublication = book.YearOfPublication,
            Language = book.Language,
            Name = book.Name,
        };
    }
}