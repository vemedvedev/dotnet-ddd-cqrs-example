using Bank.Domain.Exceptions;
using Bank.Shared.Constants.ErrorCodes;

namespace Bank.Application.Handlers.Account.Create.Exceptions;

public class AccountNameAlreadyExistsException : DomainRuleBaseException
{
    public AccountNameAlreadyExistsException() :
        base(AccountErrors.NameAlreadyUsedCode, AccountErrors.NameAlreadyUsedCodeStr,
            AccountErrors.NameAlreadyExistsUserMessage)
    {
    }
}
