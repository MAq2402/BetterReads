namespace BetterReads.Shelves.Infra.Mongo.Documents;

public class BookDocument
{
    public string Name { get; set; }
    public string Author { get; set; }
    public string Isbn { get; set; }
    public string Language { get; set; }
    public int YearOfPublication { get; set; }
}