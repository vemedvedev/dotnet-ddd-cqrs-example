namespace Bank.Api.Contracts.Account.Response;

public record AccountDetailResponse
{
    public required long Id { get; init; }
    public required string Name { get; init; }
    public required decimal AccountBalance { get; init; }
    public required Guid Uid { get; init; }
}
