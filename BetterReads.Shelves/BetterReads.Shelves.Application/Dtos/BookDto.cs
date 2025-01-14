namespace BetterReads.Shelves.Application.Dtos;

public class BookDto
{
    public string Name { get; set; }
    public string Author { get; set; }
    public string Isbn { get; set; }
    public string Language { get; set; }
    public int YearOfPublication { get; set; }
}