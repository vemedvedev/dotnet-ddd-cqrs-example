using Bank.Application.Repositories;
using Bank.Application.Shared;

namespace Bank.Application.Handlers.Account.GetListAccount;

public class GetListAccountQueryHandler : IQueryHandler<GetListAccountQuery, ICollection<AccountBriefDto>>
{
    private readonly IAccountRepository _accountRepository;

    public GetListAccountQueryHandler(IAccountRepository accountRepository)
    {
        _accountRepository = accountRepository;
    }

    public async Task<ICollection<AccountBriefDto>> HandleAsync(GetListAccountQuery input, CancellationToken ct)
    {
        var list = await _accountRepository.GetAccountsAsync(asNoTracking: true, ct);

        var accountBriefs = list.Select(l => new AccountBriefDto
        {
            Id = l.Id,
            Uid = l.Uid,
            Name = l.Name,
            AccountBalance = l.AccountBalance.Balance
        }).ToList();

        return accountBriefs;
    }
}
