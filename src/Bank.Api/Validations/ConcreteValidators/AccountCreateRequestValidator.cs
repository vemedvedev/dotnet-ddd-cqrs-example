using Bank.Api.Contracts.Account.Request;
using FluentValidation;

namespace Bank.Api.Validations.ConcreteValidators;

public class AccountCreateRequestValidator : AbstractValidator<AccountCreateRequest>
{
    public AccountCreateRequestValidator()
    {
        RuleFor(x => x.Name).NotEmpty().MaximumLength(OrderValidatorConstants.NameMaxLenght);
        RuleFor(x => x.InitialBalance).GreaterThanOrEqualTo(0);
    }
}

internal static class OrderValidatorConstants
{
    public const int NameMaxLenght = 256;
}

