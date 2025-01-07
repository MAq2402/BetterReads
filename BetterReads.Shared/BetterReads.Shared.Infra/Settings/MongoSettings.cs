namespace BetterReads.Shared.Infra.Settings;

internal sealed class MongoSettings
{
    public static string Name => "Mongo"; 
    public string ConnectionString { get; set; } = null!;
    public string DatabaseName { get; set; } = null!;
}