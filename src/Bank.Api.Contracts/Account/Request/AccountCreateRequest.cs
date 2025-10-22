namespace Bank.Api.Contracts.Account.Request;

public record AccountCreateRequest
{
    public string Name { get; init; } = string.Empty;
    public decimal? InitialBalance { get; init; }
}
