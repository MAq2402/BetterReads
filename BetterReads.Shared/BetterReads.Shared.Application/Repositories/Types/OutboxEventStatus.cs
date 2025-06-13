namespace BetterReads.Shared.Application.Repositories.Types;

public enum OutboxEventStatus
{
    New,
    Delivered,
    FailedToDeliver
}