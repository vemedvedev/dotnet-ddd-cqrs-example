using Bank.Application.Repositories;

namespace Bank.Application.Handlers.Transaction.Withdrawal;

public class WithdrawalCommandHandler : ICommandHandler<WithdrawalCommand, Guid>
{
    private readonly IAccountRepository _accountRepository;
    private readonly ITransactionRepository _transactionRepository;
    private readonly IUnitOfWork _unitOfWork;

    public WithdrawalCommandHandler(IAccountRepository accountRepository,
        ITransactionRepository transactionRepository,
        IUnitOfWork unitOfWork)
    {
        _accountRepository = accountRepository;
        _transactionRepository = transactionRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Guid> HandleAsync(WithdrawalCommand command, CancellationToken ct)
    {
        var account = await _accountRepository.GetAccountByIdAsync(command.AccountId, asNoTracking: false, ct);
        var balanceBefore = account.AccountBalance.Balance;
        account.Withdrawal(command.Amount);

        var transaction = Domain.TransactionAggregate.Transaction.MakeWithdrawal(
            account.Id,
            command.Amount,
            balanceBefore,
            account.AccountBalance.Balance);
        _transactionRepository.AddTransaction(transaction);

        await _unitOfWork.SaveChangesAsync(ct);

        return transaction.Uid;
    }
}
