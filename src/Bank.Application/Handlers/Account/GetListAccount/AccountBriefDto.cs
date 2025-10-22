namespace Bank.Application.Handlers.Account.GetListAccount;

public record AccountBriefDto
{
    public required long Id { get; init; }
    public required string Name { get; init; } = string.Empty;
    public required decimal AccountBalance { get; init; }
    public required Guid Uid { get; init; }
}
