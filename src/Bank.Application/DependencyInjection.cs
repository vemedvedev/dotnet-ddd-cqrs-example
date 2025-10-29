using Bank.Application.Handlers;
using Bank.Application.Handlers.Account.Create;
using Bank.Application.Handlers.Account.GetAccount;
using Bank.Application.Handlers.Account.GetListAccount;
using Bank.Application.Handlers.Transaction.Deposit;
using Bank.Application.Handlers.Transaction.Withdrawal;
using Bank.Application.Handlers.Transaction.Transfer;
using Bank.Application.Handlers.Transaction.Transfer.DomainEventHandlers;
using Bank.Application.Services.OptimisticConcurrency;
using Bank.Application.Shared;
using Bank.Domain.AccountAggregate.DomainEvents;
using Bank.Shared.Notifications;
using Microsoft.Extensions.DependencyInjection;

namespace Bank.Application;

public static class DependencyInjection
{
    public static void ConfigureApplicationLayer(this IServiceCollection services)
    {
        RegisterHandlers(services);
    }

    private static void RegisterHandlers(this IServiceCollection services)
    {
       services.RegisterAccountHandlers();
       services.RegisterTransactionHandlers();
    }

    private static void RegisterAccountHandlers(this IServiceCollection services)
    {
        services.AddScoped<ICommandHandler<CreateAccountCommand, long>, CreateCommandHandler>();
        services.AddScoped<IQueryHandler<GetAccountQuery, AccountDetailDto>, GetAccountQueryHandler>();
        services.AddScoped<IQueryHandler<GetListAccountQuery, ICollection<AccountBriefDto>>, GetListAccountQueryHandler>();
    }

    private static void RegisterTransactionHandlers(this IServiceCollection services)
    {
        services.AddKeyedScoped<ICommandHandler<DepositCommand, Guid>, DepositCommandHandler>(nameof(DepositCommand));
        services.AddScoped<ICommandHandler<DepositCommand, Guid>>(sp => new OptimisticConcurrencyHandlerDecorator<DepositCommand, Guid>(sp));

        services.AddKeyedScoped<ICommandHandler<WithdrawalCommand, Guid>, WithdrawalCommandHandler>(nameof(WithdrawalCommand));
        services.AddScoped<ICommandHandler<WithdrawalCommand, Guid>>(sp => new OptimisticConcurrencyHandlerDecorator<WithdrawalCommand, Guid>(sp));

        services.AddKeyedScoped<ICommandHandler<TransferCommand, Guid>, TransferCommandHandler>(nameof(TransferCommand));
        services.AddScoped<ICommandHandler<TransferCommand, Guid>>(sp => new OptimisticConcurrencyHandlerDecorator<TransferCommand, Guid>(sp));
        services.AddScoped<INotificationHandler<FundsTransferEvent>, FundsTransferHandler>();
    }
}
