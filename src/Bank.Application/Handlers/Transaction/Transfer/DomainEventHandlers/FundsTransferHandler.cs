using Bank.Domain.AccountAggregate.DomainEvents;
using Bank.Shared.Notifications;
using Microsoft.Extensions.Logging;

namespace Bank.Application.Handlers.Transaction.Transfer.DomainEventHandlers;

public class FundsTransferHandler : INotificationHandler<FundsTransferEvent>
{
    private readonly ILogger<FundsTransferHandler> _logger;

    public FundsTransferHandler(ILogger<FundsTransferHandler> logger)
    {
        _logger = logger;
    }

    public ValueTask HandleAsync(FundsTransferEvent notification, CancellationToken ct = default)
    {
        _logger.LogInformation("Funds transferred: {Amount} from Account {FromAccountId} to Account {ToAccountId}. " +
                               "From Balance Before: {FromBalanceBefore}, From Balance After: {FromBalanceAfter}",
            notification.Amount,
            notification.FromAccountId,
            notification.ToAccountId,
            notification.FromBalanceBefore,
            notification.FromBalanceAfter);

        return ValueTask.CompletedTask;
    }
}
