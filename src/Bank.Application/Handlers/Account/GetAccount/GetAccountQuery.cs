using Bank.Application.Shared;

namespace Bank.Application.Handlers.Account.GetAccount;

public record GetAccountQuery(long Id) : IQuery<AccountDetailDto>;
