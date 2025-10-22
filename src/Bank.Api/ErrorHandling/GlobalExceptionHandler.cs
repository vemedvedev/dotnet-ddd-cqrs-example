using System.Diagnostics;
using System.Net;
using Bank.Api.ErrorHandling.ValidationResponse;
using Bank.Api.Infrastructure.Exceptions;
using Bank.Domain.Exceptions;
using Bank.Infrastructure.Json;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Mvc;

namespace Bank.Api.ErrorHandling;

public sealed class GlobalExceptionHandler : IExceptionHandler
{
    private readonly ILogger<GlobalExceptionHandler> _logger;
    private readonly IHostEnvironment _env;

    public GlobalExceptionHandler(IHostEnvironment env, ILogger<GlobalExceptionHandler> logger)
    {
        _env = env;
        _logger = logger;
    }

    public async ValueTask<bool> TryHandleAsync(
        HttpContext httpContext,
        Exception exception,
        CancellationToken cancellationToken)
    {
        _logger.LogError(exception, "Exception occurred: {Message}", exception.Message);

        var problemDetails = CreateResultFromException(httpContext, exception);
        var json = JsonHelper.Serialize(problemDetails);

        const string contentType = "application/problem+json";
        httpContext.Response.ContentType = contentType;
        httpContext.Response.StatusCode = problemDetails.Status ?? StatusCodes.Status500InternalServerError;
        await httpContext.Response
            .WriteAsync(json, cancellationToken);

        return true;
    }

    private ProblemDetails CreateResultFromException(HttpContext context, Exception exception)
    {
        ProblemDetails? problemDetails;
        switch (exception)
        {
            case ApiValidationException validationException:
                _logger.LogError(validationException, "ApiValidationException fired");
                problemDetails = new ValidationErrorResponse(validationException.CommandValidationErrors);
                break;
            case DomainRuleBaseException domainException:
                _logger.LogError(domainException, "DomainRuleBaseException fired");
                problemDetails = new ApplicationErrorResponse(domainException.Code,
                    domainException.CodeStr,
                    domainException.UserMessage);
                break;
            case DomainException domainException:
                _logger.LogError(domainException, "DomainException fired");
                problemDetails = new ProblemDetails
                {
                    Title = "Something wrong",
                    Status = (int)HttpStatusCode.InternalServerError
                };
                break;
            default:
                _logger.LogCritical(exception, "Unhandled exception");

                problemDetails = new ProblemDetails
                {
                    Title = "Something wrong",
                    Status = (int)HttpStatusCode.InternalServerError
                };
                break;
        }

        if (!_env.IsProduction())
        {
            problemDetails.Detail = exception.ToString();
        }

        problemDetails.Instance = $"{context.Request.Method} {context.Request.Path}";
        problemDetails.Extensions.TryAdd("requestId", context.TraceIdentifier);

        Activity? activity = context.Features.Get<IHttpActivityFeature>()?.Activity;
        problemDetails.Extensions.TryAdd("traceId", activity?.Id);

        return problemDetails;
    }
}
