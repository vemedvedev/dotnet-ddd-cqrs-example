using Bank.Domain.Base;
using Bank.Infrastructure.EventDispatcher;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace Bank.Infrastructure.Persistence;

internal class EventDispatchInterceptor : SaveChangesInterceptor
{
    private readonly IEventDispatcherService _domainEventDispatcher;

    public EventDispatchInterceptor(IEventDispatcherService domainEventDispatcher)
    {
        _domainEventDispatcher = domainEventDispatcher;
    }

    public override async ValueTask<int> SavedChangesAsync(SaveChangesCompletedEventData eventData, int result,
        CancellationToken cancellationToken = default)
    {
        var context = eventData.Context;
        if (context is not BankDbContext appDbContext)
        {
            return await base.SavedChangesAsync(eventData, result, cancellationToken);
        }

        var entitiesWithEvents = appDbContext.ChangeTracker.Entries<BaseEntity>()
            .Select(e => e.Entity)
            .Where(e => e.DomainEvents.Count > 0)
            .ToArray();

        await _domainEventDispatcher.DispatchAndClearEvents(entitiesWithEvents, cancellationToken);

        return await base.SavedChangesAsync(eventData, result, cancellationToken);
    }
}
