using Microsoft.AspNetCore.Mvc;

namespace Bank.Api.ErrorHandling.ValidationResponse;

public class ValidationErrorResponse : ValidationProblemDetails
{
    public const string TitleConst = "Validation errors";

    public ValidationErrorResponse(IReadOnlyDictionary<string, string[]> errors) :
        base(errors.ToDictionary())
    {
        Title = TitleConst;
        Status = StatusCodes.Status400BadRequest;
    }
}
