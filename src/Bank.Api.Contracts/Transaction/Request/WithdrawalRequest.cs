namespace Bank.Api.Contracts.Transaction.Request;

public record WithdrawalRequest
{
    public long AccountId { get; init; }
    public decimal Amount { get; init; }
}
