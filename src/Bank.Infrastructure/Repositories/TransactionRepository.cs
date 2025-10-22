using Bank.Application.Repositories;
using Bank.Domain.TransactionAggregate;
using Bank.Infrastructure.Persistence;

namespace Bank.Infrastructure.Repositories;

public class TransactionRepository : ITransactionRepository
{
    private readonly BankDbContext _dbContext;

    public TransactionRepository(BankDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public void AddTransaction(Transaction transaction)
    {
        _dbContext.Transaction.Add(transaction);
    }
}
