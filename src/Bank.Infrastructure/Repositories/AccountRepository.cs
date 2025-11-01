using System.Text;
using Bank.Application.Handlers.Account.GetAccount.Exceptions;
using Bank.Application.Repositories;
using Bank.Domain.AccountAggregate;
using Bank.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Bank.Infrastructure.Repositories;

public sealed class AccountRepository : IAccountRepository
{
    private readonly BankDbContext _context;

    public AccountRepository(BankDbContext dbContext)
    {
        _context = dbContext;
    }

    public async Task<bool> IsAccountNameExistsAsync(string accountName, CancellationToken cancellationToken)
    {
        return await _context.Account
            .AsNoTracking()
            .AnyAsync(a => a.Name == accountName, cancellationToken);
    }

    /// <exception cref="AccountNotFoundException"></exception>
    public async Task<Account> GetAccountByIdAsync(long accountId, bool asNoTracking, CancellationToken cancellationToken)
    {
        IQueryable<Account> query = _context.Account;

        query = asNoTracking ? query.AsNoTracking() : query;

        var account = await query.FirstOrDefaultAsync(a => a.Id == accountId, cancellationToken);
        return account ?? throw new AccountNotFoundException(accountId);
    }

    public async Task<List<Account>> GetAccountsAsync(bool asNoTracking, CancellationToken cancellationToken)
    {
        IQueryable<Account> query = _context.Account;
        query = asNoTracking ? query.AsNoTracking() : query;

        return await query.OrderByDescending(a => a.CreatedAt).ToListAsync(cancellationToken);
    }

    public void AddAccount(Account account)
    {
        _context.Account.Add(account);
    }
}
