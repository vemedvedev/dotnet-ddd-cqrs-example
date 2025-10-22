using Bank.Domain.AccountAggregate;

namespace Bank.Application.Repositories;

public interface IAccountRepository
{
    public Task<bool> IsAccountNameExistsAsync(string accountName, CancellationToken cancellationToken);
    public Task<Account> GetAccountByIdAsync(long accountId, bool asNoTracking, CancellationToken cancellationToken);
    public Task<List<Account>> GetAccountsAsync(bool asNoTracking, CancellationToken cancellationToken);
    public void AddAccount(Account account);
}
