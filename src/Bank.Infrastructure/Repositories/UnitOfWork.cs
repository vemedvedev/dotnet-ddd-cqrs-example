using Bank.Application.Repositories;
using Bank.Infrastructure.Persistence;

namespace Bank.Infrastructure.Repositories;

public class UnitOfWork : IUnitOfWork
{
    private readonly BankDbContext _dbContext;

    public UnitOfWork(BankDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task SaveChangesAsync(CancellationToken ct)
    {
        await _dbContext.SaveChangesAsync(ct);
    }
}
