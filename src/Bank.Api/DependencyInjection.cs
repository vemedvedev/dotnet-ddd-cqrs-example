using System.Diagnostics;
using Bank.Api.ErrorHandling;
using Bank.Api.Validations;
using Bank.Api.Validations.ConcreteValidators;
using Bank.Api.Validations.Services;
using Bank.Infrastructure.Persistence;
using FluentValidation;
using Microsoft.AspNetCore.Http.Features;

namespace Bank.Api;

public static class DependencyInjection
{
    public static void ConfigureApiLayer(this IServiceCollection services)
    {
        RegisterValidators(services);
        RegisterExceptionHanders(services);
        RegisterHealthChecks(services);
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
}
