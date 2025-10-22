namespace Bank.Api.Contracts.Transaction.Request;

public record TransferRequest
{
    public long FromAccountId { get; init; }
    public long ToAccountId { get; init; }
    public decimal Amount { get; init; }
}
