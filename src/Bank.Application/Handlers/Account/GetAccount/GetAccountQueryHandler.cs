using Bank.Application.Repositories;

namespace Bank.Application.Handlers.Account.GetAccount;

public class GetAccountQueryHandler : IQueryHandler<GetAccountQuery, AccountDetailDto>
{
    private readonly IAccountRepository _accountRepository;

    public GetAccountQueryHandler(IAccountRepository accountRepository)
    {
        _accountRepository = accountRepository;
    }

    public async Task<AccountDetailDto> HandleAsync(GetAccountQuery input, CancellationToken ct)
    {
        var account = await _accountRepository.GetAccountByIdAsync(input.Id, asNoTracking: true, ct);

        return new AccountDetailDto
        {
            Id = account.Id,
            Name = account.Name,
            AccountBalance = account.AccountBalance.Balance,
            Uid = account.Uid
        };
    }
}
