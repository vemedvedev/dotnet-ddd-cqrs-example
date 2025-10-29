using Bank.Api.ErrorHandling;
using Bank.Api.Validations;
using Bank.Api.Validations.ConcreteValidators;
using Bank.Api.Validations.Services;
using Bank.Infrastructure.Persistence;
using Bank.Shared.Notifications;
using Bank.Shared.Notifications.Implementations;
using FluentValidation;

namespace Bank.Api;

public static class DependencyInjection
{
    public static void ConfigureApiLayer(this IServiceCollection services)
    {
        RegisterValidators(services);
        RegisterExceptionHanders(services);
        RegisterHealthChecks(services);
        RegisterNotification(services);
    }

    private static void RegisterValidators(this IServiceCollection services)
    {
        services.AddValidatorsFromAssemblyContaining<AccountCreateRequestValidator>();
        services.AddScoped(typeof(IValidationService<>), typeof(ValidationService<>));
    }

    private static void RegisterExceptionHanders(this IServiceCollection services)
    {
        services.AddExceptionHandler<GlobalExceptionHandler>();
        services.AddProblemDetails();
    }

    private static void RegisterHealthChecks(this IServiceCollection services)
    {
        services.AddHealthChecks()
            .AddDbContextCheck<BankDbContext>("BankDbHealthCheck");
    }

    private static void RegisterNotification(this IServiceCollection services)
    {
        services.AddScoped<INotificationPublisher, NotificationPublisher>();
    }
}
