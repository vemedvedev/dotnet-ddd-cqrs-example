using Bank.Domain.AccountAggregate.Exceptions;

namespace Bank.Application.Tests.Handlers;

public static class DomainExceptionHelper
{
    public static void CheckInsufficientFund(InsufficientFundsDomainException exception)
    {
        exception.Code.Should().Be(200);
        exception.UserMessage.Should().Be("Insufficient funds in the account.");
        exception.CodeStr.Should().Be("insufficient_funds");
    }
}
