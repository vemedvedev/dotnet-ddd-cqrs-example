using System.Diagnostics.CodeAnalysis;
using Bank.Application.Handlers;
using Bank.Application.Handlers.Transaction.Deposit;
using Bank.Application.Services.OptimisticConcurrency;
using Bank.Application.Shared;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NSubstitute;

namespace Bank.Application.Tests.Services;

[SuppressMessage("Naming", "CA1707:Identifiers should not contain underscores")]
public class OptimisticConcurrencyHandlerDecoratorTests
{
    [Fact]
    public async Task HandleAsync_NotExceedMaxRetries_Succeeds()
    {
        // Arrange
        var logger = Substitute.For<ILogger<OptimisticConcurrencyHandlerDecorator<DepositCommand, Guid>>>();
        var inner = Substitute.For<ICommandHandler<DepositCommand, Guid>>();
        int calls = 0;
        var expected = Guid.NewGuid();
        inner.HandleAsync(Arg.Any<DepositCommand>(), Arg.Any<CancellationToken>())
            .Returns(_ =>
            {
                calls++;
                if (calls < 3)
                {
                    throw new DbUpdateConcurrencyException("conflict");
                }

                return Task.FromResult(expected);
            });

        var services = new ServiceCollection();
        services.AddKeyedTransient<ICommandHandler<DepositCommand, Guid>>(nameof(DepositCommand), (_, _) => inner);
        services.AddScoped<ILogger<OptimisticConcurrencyHandlerDecorator<DepositCommand, Guid>>>(_ => logger);
        var provider = services.BuildServiceProvider();

        var decorator = new OptimisticConcurrencyHandlerDecorator<DepositCommand, Guid>(provider);
        var cmd = new DepositCommand { AccountId = 1, Amount = 1m };

        // Act
        var result = await decorator.HandleAsync(cmd, CancellationToken.None);

        // Assert
        Assert.Equal(expected, result);
        await inner.Received(3).HandleAsync(cmd, Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task HandleAsync_ExceedMaxRetries_Exception()
    {
        // Arrange
        var logger = Substitute.For<ILogger<OptimisticConcurrencyHandlerDecorator<DepositCommand, Guid>>>();
        var inner = Substitute.For<ICommandHandler<DepositCommand, Guid>>();
        var expected = Guid.NewGuid();
        inner.HandleAsync(Arg.Any<DepositCommand>(), Arg.Any<CancellationToken>())
            .Returns(_ =>
            {
                throw new DbUpdateConcurrencyException("conflict");

#pragma warning disable CS0162 // Unreachable code detected
                return Task.FromResult(expected);
#pragma warning restore CS0162 // Unreachable code detected
            });

        var services = new ServiceCollection();
        services.AddKeyedTransient<ICommandHandler<DepositCommand, Guid>>(nameof(DepositCommand), (_, _) => inner);
        services.AddScoped<ILogger<OptimisticConcurrencyHandlerDecorator<DepositCommand, Guid>>>(_ => logger);
        var provider = services.BuildServiceProvider();

        var decorator = new OptimisticConcurrencyHandlerDecorator<DepositCommand, Guid>(provider);
        var cmd = new DepositCommand { AccountId = 1, Amount = 1m };

        // Act
        await Assert.ThrowsAsync<DbUpdateConcurrencyException>(async () => await decorator.HandleAsync(cmd, CancellationToken.None));

        // Assert
        await inner.Received(3).HandleAsync(cmd, Arg.Any<CancellationToken>());
    }
}
