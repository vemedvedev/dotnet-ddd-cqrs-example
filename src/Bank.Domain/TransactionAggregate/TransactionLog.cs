using Bank.Domain.Base;

namespace Bank.Domain.TransactionAggregate;

public class TransactionLog : BaseEntity
{
    private TransactionLog()
    {
    }

    public long TransactionId { get; private init; }
    public long AccountId { get; private init; }
    public decimal Delta { get; private init; }
    public decimal BeforeBalance { get; private init; }
    public decimal AfterBalance { get; private init; }

    public static TransactionLog CreateTransactionLog(long accountId, decimal delta,
        decimal beforeBalance, decimal afterBalance)
    {
        return new TransactionLog
        {
            Delta = delta,
            BeforeBalance = beforeBalance,
            AfterBalance = afterBalance,
            AccountId = accountId
        };
    }
}
