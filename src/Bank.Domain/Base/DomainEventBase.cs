using Bank.Shared.Notifications;

namespace Bank.Domain.Base;

public abstract class DomainEventBase : INotification
{
    public DateTime CreatedAt { get; protected set; } = DateTime.UtcNow;
}
