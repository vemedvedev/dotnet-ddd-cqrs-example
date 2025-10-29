using Bank.Domain.Base;

namespace Bank.Infrastructure.EventDispatcher;

internal interface IEventDispatcherService
{
    ValueTask DispatchAndClearEvents(IEnumerable<BaseEntity> entityWithEvents, CancellationToken ct = default);
}
