using Bank.Application.Handlers.Account.Create.Exceptions;
using Bank.Application.Repositories;
using Bank.Application.Shared;

namespace Bank.Application.Handlers.Account.Create;

public class CreateCommandHandler : ICommandHandler<CreateAccountCommand, long>
{
    private readonly IAccountRepository _accountRepository;
    private readonly IUnitOfWork _unitOfWork;

    public CreateCommandHandler(IAccountRepository accountRepository,
        IUnitOfWork unitOfWork)
    {
        _accountRepository = accountRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<long> HandleAsync(CreateAccountCommand accountCommand, CancellationToken ct)
    {
        await CheckAccountNameUniqueAsync(accountCommand.Name, ct);

        var account = Domain.AccountAggregate.Account.Create(accountCommand.Name, accountCommand.InitialBalance);
        _accountRepository.AddAccount(account);
        await _unitOfWork.SaveChangesAsync(ct);

        return account.Id;
    }

    private async Task CheckAccountNameUniqueAsync(string name, CancellationToken ct)
    {
        if (await _accountRepository.IsAccountNameExistsAsync(name, ct))
        {
            throw new AccountNameAlreadyExistsException();
        }
    }
}
