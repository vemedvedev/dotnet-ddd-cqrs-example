using Microsoft.AspNetCore.Mvc;

namespace Bank.Api.ErrorHandling.ValidationResponse;

public class ApplicationErrorResponse : ValidationProblemDetails
{
    public ApplicationErrorResponse(long code, string codeStr, string userMessage)
    {
        Code = code;
        CodeStr = codeStr;
        UserMessage = userMessage;
        Status = StatusCodes.Status422UnprocessableEntity;
    }

    public long Code { get; }
    public string CodeStr { get; }
    public string UserMessage { get; }
}
