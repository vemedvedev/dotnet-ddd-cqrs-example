using Bank.Application.Handlers;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Bank.Application.Services.OptimisticConcurrency;

public class OptimisticConcurrencyHandlerDecorator<TCommand, TResult>
    : ICommandHandler<TCommand, TResult> where TCommand : ICommand<TResult>
{
    private const int MaxRetry = 3;

    private readonly IServiceProvider _serviceProvider;

    public OptimisticConcurrencyHandlerDecorator(
        IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public async Task<TResult> HandleAsync(TCommand command, CancellationToken ct)
    {
        for (int attempt = 1; ; attempt++)
        {
            using var scope = _serviceProvider.CreateScope();
            var handler = scope.ServiceProvider.GetRequiredKeyedService<ICommandHandler<TCommand, TResult>>(typeof(TCommand).Name);
            var logger = scope.ServiceProvider.GetRequiredService<ILogger<OptimisticConcurrencyHandlerDecorator<TCommand, TResult>>>();

            try
            {
                return await handler.HandleAsync(command, ct);
            }
            catch (DbUpdateConcurrencyException ex) when (attempt < MaxRetry)
            {
                logger.LogWarning(ex,
                    "Optimistic concurrency conflict while handling {Command}. Attempt {Attempt}/{MaxRetry}. Retrying...",
                    typeof(TCommand).Name, attempt, MaxRetry);
            }
        }
    }
}
