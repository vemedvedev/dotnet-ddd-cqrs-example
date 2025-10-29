using Bank.Application.Shared;

namespace Bank.Application.Handlers.Account.GetListAccount;

public record GetListAccountQuery : IQuery<ICollection<AccountBriefDto>>;
