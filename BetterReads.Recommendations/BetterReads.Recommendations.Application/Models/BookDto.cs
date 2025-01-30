using System.Text.Json.Serialization;

namespace BetterReads.Recommendations.Application.Models;

public class BookDto
{
    [JsonPropertyName("name")]
    public string Name { get; set; }
    [JsonPropertyName("author")]
    public string Author { get; set; }
    [JsonPropertyName("isbn")]
    public string Isbn { get; set; }
    [JsonPropertyName("language")]
    public string Language { get; set; }
    [JsonPropertyName("yearOfPublication")]
    public int YearOfPublication { get; set; }
}