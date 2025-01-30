using System.Text.Json.Serialization;

namespace BetterReads.Recommendations.Application.Models;

public class AiRecommendationResponse
{
    [JsonPropertyName("title")]
    public string? Title { get; set; }
    [JsonPropertyName("author")]
    public string? Author { get; set; }
    [JsonPropertyName("ISBN")]
    public string? Isbn { get; set; }
}