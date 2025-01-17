using BetterReads.Shared.Application.Dtos;
using BetterReads.Shared.Application.Exceptions;
using BetterReads.Shelves.Application.Commands;
using MediatR;

namespace BetterReads.Shelves.Application.Dtos;

public class AddBookDto : ModifyDto
{
    public string Name { get; set; } = string.Empty;
    public string Author { get; set; } = string.Empty;
    public string Isbn { get; set; } = string.Empty;
    public string Language { get; set; } = string.Empty;
    public int YearOfPublication { get; set; }
    public Guid ShelfId { get; set; }
    public IRequest ToCommand(Guid? userId = null)
    {
        if (!userId.HasValue)
        {
            throw new UnauthorizedException(nameof(userId));
        }

        return new AddBook(Name, Author, Isbn, Language, YearOfPublication, userId.Value, ShelfId);
    }
}