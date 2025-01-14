using BetterReads.Shared.Domain.Base;

namespace BetterReads.Shelves.Domain;

public record Book
{
    public Book(string name, string author, string isbn, string language, int yearOfPublication)
    {
        Name = name;
        Author = author;
        Isbn = isbn;
        Language = language;
        YearOfPublication = yearOfPublication;
    }

    public string Name { get; private set; }
    public string Author { get; private set; }
    public string Isbn { get; private set; }
    public string Language { get; private set; }
    public int YearOfPublication { get; private set; }
}