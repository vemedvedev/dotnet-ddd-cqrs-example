using Bank.Application.Shared;

namespace Bank.Application.Handlers.Transaction.Transfer;

public record TransferCommand : ICommand<Guid>
{
    public required long FromAccountId { get; init; }
    public required long ToAccountId { get; init; }
    public required decimal Amount { get; init; }
}
