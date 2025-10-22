namespace Bank.Application.Handlers.Transaction.Deposit;

public record DepositCommand : ICommand<Guid>
{
    public required long AccountId { get; init; }
    public required decimal Amount { get; init; }
}
