namespace Bank.Api.Contracts.Transaction.Request;

public record DepositRequest
{
    public long AccountId { get; init; }
    public decimal Amount { get; init; }
}
