using Bank.Domain.Base;

namespace Bank.Domain.AccountAggregate.DomainEvents;

public class FundsTransferEvent : DomainEventBase
{
    public required long ToAccountId { get; init; }
    public required decimal Amount { get; init; }
    public required long FromAccountId { get; init; }
    public required decimal FromBalanceBefore { get; init; }
    public required decimal FromBalanceAfter { get; init; }
    public required decimal ToBalanceBefore { get; init; }
    public required decimal ToBalanceAfter { get; init; }
}
