using Bank.Domain.Base;
using Bank.Domain.Exceptions;

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.

namespace Bank.Domain.AccountAggregate;

public class Account : BaseEntity, IAggregateRoot
{
    private Account()
    {
    }

    public static Account Create(string name, decimal? initialBalance)
    {
        if (initialBalance < 0)
        {
            throw new DomainException("Initial balance must be >= 0");
        }

        initialBalance ??= 0;

        var acc = new Account
        {
            Name = name,
            Uid = Guid.NewGuid(),
            AccountBalance = new AccountBalance(initialBalance.Value),
        };

        return acc;
    }

    public void Deposit(decimal amount)
    {
        AccountBalance.Increase(amount);
    }

    public void Withdrawal(decimal amount)
    {
        AccountBalance.Decrease(amount);
    }

    public AccountBalance AccountBalance { get; private init; }

    public string Name { get; private init; }
    public Guid Uid { get; private init; }
}
