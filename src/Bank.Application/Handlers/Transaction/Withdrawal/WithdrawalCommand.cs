using Bank.Application.Shared;

namespace Bank.Application.Handlers.Transaction.Withdrawal;

public record WithdrawalCommand : ICommand<Guid>
{
    public required long AccountId { get; init; }
    public required decimal Amount { get; init; }
}
