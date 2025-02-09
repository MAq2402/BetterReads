using System.Text.Json.Serialization;

public class SearchResult
{
    [JsonPropertyName("start")] public int Start { get; set; }

    [JsonPropertyName("num_found")] public int NumFound { get; set; }

    [JsonPropertyName("docs")] public List<BookDocument> Docs { get; set; } = [];
}

public class BookDocument
{
    [JsonPropertyName("cover_i")] public int? CoverId { get; set; }

    [JsonPropertyName("has_fulltext")] public bool HasFullText { get; set; }

    [JsonPropertyName("edition_count")] public int EditionCount { get; set; }
    [JsonPropertyName("title")] public string? Title { get; set; }
    [JsonPropertyName("author_name")] public List<string> AuthorName { get; set; } = [];
    [JsonPropertyName("first_publish_year")] public int? FirstPublishYear { get; set; }

    [JsonPropertyName("key")] public string? Key { get; set; }

    [JsonPropertyName("ia")] public List<string> Ia { get; set; } = [];

    [JsonPropertyName("author_key")] public List<string> AuthorKey { get; set; } = [];

    [JsonPropertyName("public_scan_b")] public bool PublicScanB { get; set; }
}