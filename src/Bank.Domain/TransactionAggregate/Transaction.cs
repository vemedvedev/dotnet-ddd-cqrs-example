using Bank.Domain.AccountAggregate.DomainEvents;
using Bank.Domain.Base;

namespace Bank.Domain.TransactionAggregate;

public class Transaction : BaseEntity, IAggregateRoot
{
    private Transaction()
    {
        _transactionLogs = [];
    }

    public Guid Uid { get; private init; }
    public TransactionType Type { get; private init; }

    private readonly List<TransactionLog> _transactionLogs;
    public IReadOnlyCollection<TransactionLog> TransactionLogs => _transactionLogs;


    public static Transaction MakeDeposit(long accountId, decimal amount,
        decimal beforeBalance, decimal afterBalance)
    {
        var tr = new Transaction
        {
            Uid = Guid.NewGuid(),
            Type = TransactionType.Deposit,
            _transactionLogs = { TransactionLog.CreateTransactionLog(accountId, amount, beforeBalance, afterBalance) }
        };

        return tr;
    }

    public static Transaction MakeWithdrawal(long accountId, decimal amount,
        decimal beforeBalance, decimal afterBalance)
    {
        var tr = new Transaction
        {
            Uid = Guid.NewGuid(),
            Type = TransactionType.Withdrawal,
            _transactionLogs = { TransactionLog.CreateTransactionLog(accountId, -amount, beforeBalance, afterBalance) }
        };

        return tr;
    }

    public static Transaction MakeTransfer(long fromAccountId, decimal fromBefore, decimal fromAfter,
        long toAccountId, decimal toBefore, decimal toAfter,
        decimal amount)
    {
        var tr = new Transaction
        {
            Uid = Guid.NewGuid(),
            Type = TransactionType.Transfer,
            _transactionLogs =
            {
                TransactionLog.CreateTransactionLog(fromAccountId, -amount, fromBefore, fromAfter),
                TransactionLog.CreateTransactionLog(toAccountId, amount, toBefore, toAfter)
            }
        };

        tr.AddDomainEvent(new FundsTransferEvent
        {
            FromAccountId = fromAccountId,
            ToAccountId = toAccountId,
            Amount = amount,
            FromBalanceBefore = fromBefore,
            FromBalanceAfter = fromAfter,
            ToBalanceBefore = toBefore,
            ToBalanceAfter = toAfter
        });

        return tr;
    }
}

public enum TransactionType : byte
{
    Deposit = 1,
    Withdrawal = 2,
    Transfer = 3
}
