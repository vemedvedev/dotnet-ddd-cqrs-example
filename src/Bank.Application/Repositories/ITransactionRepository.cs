using Bank.Domain.TransactionAggregate;

namespace Bank.Application.Repositories;

public interface ITransactionRepository
{
    public void AddTransaction(Transaction transaction);
}
