using Bank.Api.Infrastructure.Exceptions;
using FluentValidation;

namespace Bank.Api.Validations.Services;

public class ValidationService<TRequest> : IValidationService<TRequest>
{
    private readonly IValidator<TRequest> _validator;

    public ValidationService(IValidator<TRequest> validator)
    {
        _validator = validator;
    }

    /// <exception cref="ApiValidationException"></exception>
    public void Validate(TRequest request)
    {
        var validationResult = _validator.Validate(request);

        if (!validationResult.IsValid)
        {
            throw new ApiValidationException(validationResult.Errors);
        }
    }
}
