using Bank.Domain.Exceptions;
using Bank.Shared.Constants.ErrorCodes;

namespace Bank.Application.Handlers.Account.GetAccount.Exceptions;

public class AccountNotFoundException : DomainRuleBaseException
{
    public AccountNotFoundException(long accountId) :
        base(AccountErrors.AccountNotFoundCode, AccountErrors.AccountNotFoundCodeStr,
            AccountErrors.AccountNotFoundUserMessage, $"Account with id '{accountId}' was not found.")
    {
    }
}
