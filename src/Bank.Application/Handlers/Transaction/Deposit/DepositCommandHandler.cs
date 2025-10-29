using Bank.Application.Repositories;
using Bank.Application.Shared;

namespace Bank.Application.Handlers.Transaction.Deposit;

public class DepositCommandHandler : ICommandHandler<DepositCommand, Guid>
{
    private readonly IAccountRepository _accountRepository;
    private readonly ITransactionRepository _transactionRepository;
    private readonly IUnitOfWork _unitOfWork;

    public DepositCommandHandler(IAccountRepository accountRepository,
        ITransactionRepository transactionRepository,
        IUnitOfWork unitOfWork)
    {
        _accountRepository = accountRepository;
        _transactionRepository = transactionRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Guid> HandleAsync(DepositCommand command, CancellationToken ct)
    {
        var account = await _accountRepository.GetAccountByIdAsync(command.AccountId, asNoTracking: false, ct);
        var balanceBefore = account.AccountBalance.Balance;
        account.Deposit(command.Amount);

        var transaction = Domain.TransactionAggregate.Transaction.MakeDeposit(
            account.Id,
            command.Amount,
            balanceBefore,
            account.AccountBalance.Balance);
        _transactionRepository.AddTransaction(transaction);

        await _unitOfWork.SaveChangesAsync(ct);

        return transaction.Uid;
    }
}
