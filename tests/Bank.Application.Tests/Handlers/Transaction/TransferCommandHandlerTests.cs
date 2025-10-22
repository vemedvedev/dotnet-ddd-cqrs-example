using System.Diagnostics.CodeAnalysis;
using Bank.Application.Handlers.Transaction.Transfer;
using Bank.Application.Repositories;
using Bank.Domain.AccountAggregate;
using Bank.Domain.AccountAggregate.Exceptions;
using Bank.Domain.TransactionAggregate;
using NSubstitute;

using TransactionDomain = Bank.Domain.TransactionAggregate.Transaction;

namespace Bank.Application.Tests.Handlers.Transaction;

[SuppressMessage("Naming", "CA1707:Identifiers should not contain underscores")]
public class TransferCommandHandlerTests
{
    [Fact]
    public async Task HandleAsync_TransferEnoughMoney_CorrectBalanceAndTransaction()
    {
        // Arrange
        var transferAmount = 30m;
        var fromStartBalance = 100m;
        var toStartBalance = 10m;
        var from = Account.Create("From", fromStartBalance);
        var to = Account.Create("To", toStartBalance);

        var accountRepo = Substitute.For<IAccountRepository>();
        var trRepo = Substitute.For<ITransactionRepository>();
        var uow = Substitute.For<IUnitOfWork>();

        accountRepo.GetAccountByIdAsync(Arg.Is<long>(id => id == 1),
                Arg.Any<bool>(), Arg.Any<CancellationToken>())
            .Returns(_ => from);

        accountRepo.GetAccountByIdAsync(Arg.Is<long>(id => id == 2),
                Arg.Any<bool>(), Arg.Any<CancellationToken>())
            .Returns(_ => to);

        TransactionDomain? capturedTr = null;
        trRepo
            .When(r => r.AddTransaction(Arg.Any<TransactionDomain>()))
            .Do(ci => capturedTr = ci.Arg<TransactionDomain>());

        var handler = new TransferCommandHandler(accountRepo, trRepo, uow);
        var cmd = new TransferCommand
        {
            FromAccountId = 1,
            ToAccountId = 2,
            Amount = transferAmount
        };

        // Act
        var uid = await handler.HandleAsync(cmd, CancellationToken.None);

        // Assert
        await accountRepo.Received(1)
            .GetAccountByIdAsync(cmd.FromAccountId, asNoTracking: false, Arg.Any<CancellationToken>());
        await accountRepo.Received(1)
            .GetAccountByIdAsync(cmd.ToAccountId, asNoTracking: false, Arg.Any<CancellationToken>());
        trRepo.Received(1).AddTransaction(Arg.Any<TransactionDomain>());
        await uow.Received(1).SaveChangesAsync(Arg.Any<CancellationToken>());

        capturedTr.Should().NotBeNull();
        capturedTr!.Uid.Should().Be(uid);
        capturedTr!.Uid.Should().Be(uid);
        capturedTr.Type.Should().Be(TransactionType.Transfer);
        capturedTr.TransactionLogs.Should().HaveCount(2);

        var fromLog = capturedTr.TransactionLogs.Should().ContainSingle(l => l.Delta < 0).Subject;
        var toLog = capturedTr.TransactionLogs.Should().ContainSingle(l => l.Delta > 0).Subject;

        var expectedFromAfterBalance = fromStartBalance - transferAmount; // 70
        var expectedToAfterBalance = toStartBalance + transferAmount; // 40

        fromLog.AccountId.Should().Be(from.Id);
        fromLog.Delta.Should().Be(-transferAmount);
        fromLog.BeforeBalance.Should().Be(fromStartBalance);
        fromLog.AfterBalance.Should().Be(expectedFromAfterBalance);

        toLog.AccountId.Should().Be(to.Id);
        toLog.Delta.Should().Be(transferAmount);
        toLog.BeforeBalance.Should().Be(toStartBalance);
        toLog.AfterBalance.Should().Be(expectedToAfterBalance);

        from.AccountBalance.Balance.Should().Be(70m);
        to.AccountBalance.Balance.Should().Be(40m);
    }

    [Fact]
    public async Task HandleAsync_TransferNotEnoughMoney_FiredException()
    {
        // Arrange
        var transferAmount = 150m;
        var fromStartBalance = 100m;
        var toStartBalance = 10m;
        var from = Account.Create("From", fromStartBalance);
        var to = Account.Create("To", toStartBalance);

        var accountRepo = Substitute.For<IAccountRepository>();
        var trRepo = Substitute.For<ITransactionRepository>();
        var uow = Substitute.For<IUnitOfWork>();

        accountRepo.GetAccountByIdAsync(Arg.Is<long>(id => id == 1),
                Arg.Any<bool>(), Arg.Any<CancellationToken>())
            .Returns(_ => from);

        accountRepo.GetAccountByIdAsync(Arg.Is<long>(id => id == 2),
                Arg.Any<bool>(), Arg.Any<CancellationToken>())
            .Returns(_ => to);

        var handler = new TransferCommandHandler(accountRepo, trRepo, uow);
        var cmd = new TransferCommand
        {
            FromAccountId = 1,
            ToAccountId = 2,
            Amount = transferAmount
        };

        // Act
        var ex = await Assert.ThrowsAsync<InsufficientFundsDomainException>( async () =>
        {
            await handler.HandleAsync(cmd, CancellationToken.None);
        });


        // Assert
        DomainExceptionHelper.CheckInsufficientFund(ex);

        trRepo.DidNotReceiveWithAnyArgs().AddTransaction(default!);
        await uow.DidNotReceiveWithAnyArgs().SaveChangesAsync(default);
    }
}
