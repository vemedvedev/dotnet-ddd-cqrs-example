namespace Bank.Api.Contracts.Account.Response;

public record AccountListResponse
{
   public required ICollection<AccountBriefResponse> Accounts { get; init; }
}

public record AccountBriefResponse
{
    public required long Id { get; init; }
    public required string Name { get; init; }
    public required decimal AccountBalance { get; init; }
    public required Guid Uid { get; init; }
}
