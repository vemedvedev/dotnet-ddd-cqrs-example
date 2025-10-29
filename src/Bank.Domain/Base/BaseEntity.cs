using System.ComponentModel.DataAnnotations.Schema;

namespace Bank.Domain.Base;

public abstract class BaseEntity
{
    private readonly List<DomainEventBase> _domainEvents = new();
    public long Id { get; private set; }
    public DateTime CreatedAt { get; private set; }

    [NotMapped]
    public IReadOnlyCollection<DomainEventBase> DomainEvents => _domainEvents.AsReadOnly();

    protected void AddDomainEvent(DomainEventBase domainEvent)
    {
        _domainEvents.Add(domainEvent);
    }

    public void CleanUpDomainEvents() => _domainEvents.Clear();
}
