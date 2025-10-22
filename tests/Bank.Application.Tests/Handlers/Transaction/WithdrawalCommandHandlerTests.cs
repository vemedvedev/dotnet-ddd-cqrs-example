using System.Diagnostics.CodeAnalysis;
using Bank.Application.Handlers.Transaction.Withdrawal;
using Bank.Application.Repositories;
using Bank.Domain.AccountAggregate;
using Bank.Domain.AccountAggregate.Exceptions;
using NSubstitute;
using TransactionDomain = Bank.Domain.TransactionAggregate.Transaction;

namespace Bank.Application.Tests.Handlers.Transaction;

[SuppressMessage("Naming", "CA1707:Identifiers should not contain underscores")]
public class WithdrawalCommandHandlerTests
{
    [Fact]
    public async Task HandleAsync_WithdrawalEnoughMoney_CorrectBalanceAndTransaction()
    {
        // Arrange
        var initialBalance = 100m;
        var withdrawalAmount = 10m;

        var account = Account.Create("test", initialBalance);
        var accountRepo = Substitute.For<IAccountRepository>();
        var trRepo = Substitute.For<ITransactionRepository>();
        var uow = Substitute.For<IUnitOfWork>();

        accountRepo.GetAccountByIdAsync(Arg.Any<long>(),
                Arg.Any<bool>(), Arg.Any<CancellationToken>())
            .Returns(_ => account);

        TransactionDomain? capturedTr = null;

        trRepo.When(r => r.AddTransaction(Arg.Any<TransactionDomain>()))
            .Do(ci => capturedTr = ci.Arg<TransactionDomain>());

        var handler = new WithdrawalCommandHandler(accountRepo, trRepo, uow);
        var cmd = new WithdrawalCommand { AccountId = 1, Amount = withdrawalAmount };

        // Act
        var uid = await handler.HandleAsync(cmd, CancellationToken.None);

        // Assert
        capturedTr!.Uid.Should().Be(uid);
        capturedTr.Uid.Should().NotBe(Guid.Empty);
        capturedTr.Type.Should().Be(Domain.TransactionAggregate.TransactionType.Withdrawal);
        capturedTr.TransactionLogs.Should().HaveCount(1);

        var afterBalance = initialBalance - withdrawalAmount;

        var trLog = capturedTr.TransactionLogs.Should().ContainSingle().Subject;

        trLog.AfterBalance.Should().Be(afterBalance);
        trLog.BeforeBalance.Should().Be(initialBalance);
        trLog.Delta.Should().Be(-withdrawalAmount);
    }

    [Fact]
    public async Task HandleAsync_WithdrawalNotEnoughMoney_FiredException()
    {
        // Arrange
        var initialBalance = 30m;
        var withdrawalAmount = 50m;

        var account = Account.Create("test", initialBalance);
        var accountRepo = Substitute.For<IAccountRepository>();
        var trRepo = Substitute.For<ITransactionRepository>();
        var uow = Substitute.For<IUnitOfWork>();

        accountRepo.GetAccountByIdAsync(Arg.Any<long>(),
                Arg.Any<bool>(), Arg.Any<CancellationToken>())
            .Returns(_ => account);

        var handler = new WithdrawalCommandHandler(accountRepo, trRepo, uow);
        var cmd = new WithdrawalCommand { AccountId = 1, Amount = withdrawalAmount };

        // Act
        var ex = await Assert.ThrowsAsync<InsufficientFundsDomainException>(async () =>
        {
            await handler.HandleAsync(cmd, CancellationToken.None);
        });

        // Assert

        DomainExceptionHelper.CheckInsufficientFund(ex);
        trRepo.DidNotReceiveWithAnyArgs().AddTransaction(Arg.Any<TransactionDomain>());
        await uow.DidNotReceiveWithAnyArgs().SaveChangesAsync(default);
    }
}
