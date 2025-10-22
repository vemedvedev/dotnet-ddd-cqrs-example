using System.Diagnostics.CodeAnalysis;
using Bank.Application.Handlers.Transaction.Deposit;
using Bank.Application.Repositories;
using Bank.Domain.AccountAggregate;
using Bank.Domain.TransactionAggregate;
using NSubstitute;

using TransactionDomain = Bank.Domain.TransactionAggregate.Transaction;

namespace Bank.Application.Tests.Handlers.Transaction;

[SuppressMessage("Naming", "CA1707:Identifiers should not contain underscores")]
public class DepositCommandHandlerTests
{
    [Fact]
    public async Task HandleAsync_DepositMoney_CorrectBalanceAndTransaction()
    {
        // Arrange
        var depositAmount = 25m;
        var initialBalance = 100m;

        var account = Account.Create("test1", 100m);
        var accountRepo = Substitute.For<IAccountRepository>();
        var trRepo = Substitute.For<ITransactionRepository>();
        var uow = Substitute.For<IUnitOfWork>();

        accountRepo.GetAccountByIdAsync(Arg.Any<long>(), Arg.Any<bool>(),
                Arg.Any<CancellationToken>())
            .Returns(_ => account);

        TransactionDomain? capturedTr = null;
        trRepo
            .When(r => r.AddTransaction(Arg.Any<TransactionDomain>()))
            .Do(ci => capturedTr = ci.Arg<TransactionDomain>());

        var handler = new DepositCommandHandler(accountRepo, trRepo, uow);

        var cmd = new DepositCommand
        {
            AccountId = 1,
            Amount = depositAmount
        };

        // Act
        var uid = await handler.HandleAsync(cmd, CancellationToken.None);

        // Assert
        await accountRepo.Received(1)
            .GetAccountByIdAsync(cmd.AccountId, asNoTracking: false, Arg.Any<CancellationToken>());
        trRepo.Received(1).AddTransaction(Arg.Any<TransactionDomain>());
        await uow.Received(1).SaveChangesAsync(Arg.Any<CancellationToken>());

        capturedTr.Should().NotBeNull();
        capturedTr!.Uid.Should().Be(uid);
        capturedTr.Uid.Should().NotBe(Guid.Empty);
        capturedTr.Type.Should().Be(TransactionType.Deposit);
        capturedTr.TransactionLogs.Should().HaveCount(1);

        var afterBalance = initialBalance + depositAmount;

        var log = capturedTr.TransactionLogs.Should().ContainSingle().Subject;
        log.AccountId.Should().Be(account.Id);
        log.Delta.Should().Be(cmd.Amount);
        log.BeforeBalance.Should().Be(initialBalance);
        log.AfterBalance.Should().Be(afterBalance);

        account.AccountBalance.Balance.Should().Be(afterBalance);
    }
}
