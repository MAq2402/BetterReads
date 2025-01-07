namespace BetterReads.Shared.Domain.Base;

public abstract class AggregateRoot : Entity<AggregateId>
{
        
    private readonly ISet<IDomainEvent> _events = new HashSet<IDomainEvent>();
    public IEnumerable<IDomainEvent> Events => _events;
    public int Version { get; protected set; }

    protected AggregateRoot(AggregateId id) : base(id)
    {
    }

    protected void AddEvent(IDomainEvent @event)
    {
        if (!_events.Any())
        {
            Version++;
        }
                
        _events.Add(@event);
    }

    public void ClearEvents() => _events.Clear();
}