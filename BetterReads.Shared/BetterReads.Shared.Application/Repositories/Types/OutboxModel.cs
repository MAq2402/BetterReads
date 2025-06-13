namespace BetterReads.Shared.Application.Repositories.Types;

public class OutboxModel
{
    public Guid Id { get; set; }
    public string Type { get; set; } = string.Empty;
    public string Event { get; set; } = string.Empty;
    public int Version { get; set; }
    public OutboxEventStatus Status { get; set; }
    public string? ErrorMessage { get; set; }
}