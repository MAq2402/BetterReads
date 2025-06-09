namespace BetterReads.Auth.Infra.Options;

public class IdentityOptions
{
    public required string ClientSecret { get; set; }
    public required string ClientId { get; set; }
    public required string RedirectUri { get; set; }
    public required string Url { get; set; }
    public required string Region { get; set; }
}