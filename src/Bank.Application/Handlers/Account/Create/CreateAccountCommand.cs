namespace Bank.Application.Handlers.Account.Create;

public record CreateAccountCommand : ICommand<long>
{
    public string Name { get; init; } = string.Empty;
    public decimal? InitialBalance { get; init; }
}
