using Bank.Application.Repositories;
using Bank.Infrastructure.EventDispatcher;
using Bank.Infrastructure.Persistence;
using Bank.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Bank.Infrastructure;

public static class DependencyInjection
{
    public static void ConfigureInfrastructureLayer(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<EventDispatchInterceptor>();
        services.AddScoped<IEventDispatcherService, EventDispatcherService>();

        services.AddDbContext<BankDbContext>((sp, options) =>
        {
            options.UseNpgsql(configuration.GetConnectionString(nameof(BankDbContext)))
                   .AddInterceptors(sp.GetRequiredService<EventDispatchInterceptor>());
        });

        services.AddScoped<IAccountRepository, AccountRepository>();
        services.AddScoped<ITransactionRepository, TransactionRepository>();
        services.AddScoped<IUnitOfWork, UnitOfWork>();
    }
}
