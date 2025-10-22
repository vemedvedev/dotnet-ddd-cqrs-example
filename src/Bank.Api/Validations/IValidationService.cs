using Bank.Api.Infrastructure.Exceptions;

namespace Bank.Api.Validations;

public interface IValidationService<in TRequest>
{
    /// <exception cref="ApiValidationException"></exception>
    void Validate(TRequest request);
}
