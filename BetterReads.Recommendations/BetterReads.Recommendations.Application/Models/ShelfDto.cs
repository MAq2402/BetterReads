using System.Text.Json.Serialization;

namespace BetterReads.Recommendations.Application.Models;

public class ShelfDto
{
    [JsonPropertyName("id")]
    public Guid Id { get; set; }
    [JsonPropertyName("userId")]
    public Guid UserId { get; set; }
    [JsonPropertyName("name")]
    public string Name { get; set; }
    [JsonPropertyName("books")]
    public List<BookDto> Books { get; set; } = new ();
}