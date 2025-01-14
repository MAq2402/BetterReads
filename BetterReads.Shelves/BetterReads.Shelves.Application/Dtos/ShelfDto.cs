namespace BetterReads.Shelves.Application.Dtos;

public class ShelfDto
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public string Name { get; set; }
    public List<BookDto> Books { get; set; }
}