namespace BetterReads.Shared.Infra.Repositories.Types;

public enum OutboxEventStatus
{
    New,
    Delivered,
    FailedToDeliver
}