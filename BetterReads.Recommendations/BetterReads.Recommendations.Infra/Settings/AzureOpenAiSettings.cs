namespace BetterReads.Recommendations.Infra.Settings;

public class AzureOpenAiSettings
{
    public required string ApiKey { get; set; }
    public required string Endpoint { get; set; }
    public required string Deployment { get; set; }
    public string? ModelId { get; set; }
}