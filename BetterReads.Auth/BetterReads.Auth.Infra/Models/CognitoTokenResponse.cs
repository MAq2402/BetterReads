namespace BetterReads.Auth.Infra.Models;

public class CognitoTokenResponse
{
    public string access_token { get; set; }
    public string refresh_token { get; set; }
    public string id_token { get; set; }
}