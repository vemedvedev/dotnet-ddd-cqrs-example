using Bank.Api.Contracts.Transaction.Request;
using FluentValidation;

// ReSharper disable UnusedType.Global

namespace Bank.Api.Validations.ConcreteValidators;

public class TransferRequestValidator : AbstractValidator<TransferRequest>
{
    public TransferRequestValidator()
    {
        RuleFor(x => x.FromAccountId).GreaterThan(0);
        RuleFor(x => x.ToAccountId).GreaterThan(0);
        RuleFor(x => x.Amount).GreaterThan(0).LessThan(TransferRequestValidatorConstants.MaxAmount);
    }
}

internal static class TransferRequestValidatorConstants
{
    public const int MaxAmount = 10_000_000;
}

