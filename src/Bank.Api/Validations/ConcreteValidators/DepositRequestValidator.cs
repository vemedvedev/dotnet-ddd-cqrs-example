using Bank.Api.Contracts.Transaction.Request;
using FluentValidation;

// ReSharper disable UnusedType.Global

namespace Bank.Api.Validations.ConcreteValidators;

public class DepositCreateRequestValidator : AbstractValidator<DepositRequest>
{
    public DepositCreateRequestValidator()
    {
        RuleFor(x => x.AccountId).GreaterThan(0);
        RuleFor(x => x.Amount).GreaterThan(0).LessThan(DepositValidatorConstants.MaxAmount);
    }
}

internal static class DepositValidatorConstants
{
    public const int MaxAmount = 10_000_000;
}

