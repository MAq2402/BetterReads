using BetterReads.Shared.Infra.Repositories.Types;

namespace BetterReads.Shared.Infra.Documents;

public class OutboxDocument : IMongoDocument<Guid>
{
    public static string CollectionName() => "Outbox";
    public Guid Id { get; set; }
    public string? Type { get; set; }
    public string? Event { get; set; }
    public int Version { get; set; }
    public OutboxEventStatus Status { get; set; }
    public string? ErrorMessage { get; set; }
}