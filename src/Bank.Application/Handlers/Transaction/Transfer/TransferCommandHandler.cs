using Bank.Application.Repositories;

namespace Bank.Application.Handlers.Transaction.Transfer;

public class TransferCommandHandler : ICommandHandler<TransferCommand, Guid>
{
    private readonly IAccountRepository _accountRepository;
    private readonly ITransactionRepository _transactionRepository;
    private readonly IUnitOfWork _unitOfWork;

    public TransferCommandHandler(
        IAccountRepository accountRepository,
        ITransactionRepository transactionRepository,
        IUnitOfWork unitOfWork)
    {
        _accountRepository = accountRepository;
        _transactionRepository = transactionRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Guid> HandleAsync(TransferCommand command, CancellationToken ct)
    {
        var fromAccount = await _accountRepository.GetAccountByIdAsync(command.FromAccountId, asNoTracking: false, ct);
        var toAccount = await _accountRepository.GetAccountByIdAsync(command.ToAccountId, asNoTracking: false, ct);

        var fromBefore = fromAccount.AccountBalance.Balance;
        var toBefore = toAccount.AccountBalance.Balance;

        fromAccount.Withdrawal(command.Amount);
        toAccount.Deposit(command.Amount);

        var transaction = Domain.TransactionAggregate.Transaction.MakeTransfer(
            fromAccount.Id, fromBefore, fromAccount.AccountBalance.Balance,
            toAccount.Id, toBefore, toAccount.AccountBalance.Balance,
            command.Amount);

        _transactionRepository.AddTransaction(transaction);
        await _unitOfWork.SaveChangesAsync(ct);

        return transaction.Uid;
    }
}
