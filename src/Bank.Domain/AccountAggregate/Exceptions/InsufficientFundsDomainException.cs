using Bank.Domain.Exceptions;
using Bank.Shared.Constants.ErrorCodes;

namespace Bank.Domain.AccountAggregate.Exceptions;

public class InsufficientFundsDomainException : DomainRuleBaseException
{
    public InsufficientFundsDomainException(string internalMessage) :
        base(AccountBalanceErrors.InsufficientFundsCode, AccountBalanceErrors.InsufficientFundsCodeStr,
            AccountBalanceErrors.InsufficientFundsUserMessage, internalMessage)
    {
    }
}
