namespace Bank.Application.Handlers.Account.GetAccount;

public record AccountDetailDto
{
    public required long Id { get; init; }
    public required string Name { get; init; }
    public required decimal AccountBalance { get; init; }
    public required Guid Uid { get; init; }
}
