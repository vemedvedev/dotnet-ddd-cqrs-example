using Bank.Domain.AccountAggregate.Exceptions;
using Bank.Domain.Base;
using Bank.Domain.Exceptions;

namespace Bank.Domain.AccountAggregate;

public class AccountBalance : BaseEntity, IAggregateRoot
{
    public AccountBalance(decimal balance)
    {
        Balance = balance;
    }

    public decimal Balance { get; private set; }
    public uint Version { get; private set; }
    public long AccountId { get; private set; }

    public void Increase(decimal amount)
    {
        CheckOnNegative(amount);

        Balance += amount;
    }

    public void Decrease(decimal amount)
    {
        CheckOnNegative(amount);
        if (Balance < amount) throw new InsufficientFundsDomainException($"Insufficient funds: {Balance} available, {amount} requested.");

        Balance -= amount;
    }

    private static void CheckOnNegative(decimal amount)
    {
        if (amount <= 0)
        {
            throw new DomainException("Amount must be > 0");
        }
    }
}
