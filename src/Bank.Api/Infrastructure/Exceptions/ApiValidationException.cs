using FluentValidation.Results;

namespace Bank.Api.Infrastructure.Exceptions;

public class ApiValidationException : Exception
{
    private readonly IDictionary<string, string[]> _commandValidationErrors;

    public ApiValidationException(List<ValidationFailure> validationFailures)
    {
        var errorResults = validationFailures
            .GroupBy(
                x => x.PropertyName,
                x => x.ErrorMessage,
                (propertyName, errorMessages) => new
                {
                    Key = propertyName,
                    Values = errorMessages.Distinct().ToArray()
                })
            .ToDictionary(x => x.Key, x => x.Values);

        _commandValidationErrors = errorResults;
    }

    public IReadOnlyDictionary<string, string[]> CommandValidationErrors => _commandValidationErrors.AsReadOnly();
}
