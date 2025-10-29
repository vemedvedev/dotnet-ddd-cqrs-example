using Bank.Domain.Base;
using Bank.Shared.Notifications;

namespace Bank.Infrastructure.EventDispatcher;

internal class EventDispatcherService : IEventDispatcherService
{
    private readonly INotificationPublisher _notificationPublisher;

    public EventDispatcherService(INotificationPublisher notificationPublisher)
    {
        _notificationPublisher = notificationPublisher;
    }

    public async ValueTask DispatchAndClearEvents(IEnumerable<BaseEntity> entityWithEvents, CancellationToken ct = default)
    {
        foreach (var entityWithEvent in entityWithEvents)
        {
            var events = entityWithEvent.DomainEvents.ToList();
            entityWithEvent.CleanUpDomainEvents();

            foreach (var domainEvent in events)
            {
                await _notificationPublisher.PublishAsync(domainEvent, ct);
            }
        }
    }
}
