namespace BetterReads.Auth.Infra.Options;

public class IdentityOptions
{
    public string ClientSecret { get; set; }
    public string ClientId { get; set; }
    public string RedirectUri { get; set; }
    public string Url { get; set; }
}