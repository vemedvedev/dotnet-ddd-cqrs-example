using Bank.Api.Contracts.Transaction.Request;
using FluentValidation;

// ReSharper disable UnusedType.Global

namespace Bank.Api.Validations.ConcreteValidators;

public class WithdrawalRequestValidator : AbstractValidator<WithdrawalRequest>
{
    public WithdrawalRequestValidator()
    {
        RuleFor(x => x.AccountId).GreaterThan(0);
        RuleFor(x => x.Amount).GreaterThan(0).LessThan(WithdrawalRequestValidatorConstants.MaxAmount);
    }
}

internal static class WithdrawalRequestValidatorConstants
{
    public const int MaxAmount = 10_000_000;
}

