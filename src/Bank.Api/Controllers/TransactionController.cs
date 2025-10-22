using Bank.Api.Contracts.Transaction.Request;
using Bank.Api.ErrorHandling.ValidationResponse;
using Bank.Api.Mappers;
using Bank.Api.Validations;
using Bank.Application.Handlers;
using Bank.Application.Handlers.Transaction.Deposit;
using Bank.Application.Handlers.Transaction.Withdrawal;
using Bank.Application.Handlers.Transaction.Transfer;
using Microsoft.AspNetCore.Mvc;

namespace Bank.Api.Controllers;

[Route("[area]/transactions")]
public class TransactionController : BaseApiController
{
    [HttpPost("deposit")]
    [ProducesResponseType(typeof(Guid), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ValidationErrorResponse), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Deposit(
        [FromServices] ICommandHandler<DepositCommand, Guid> handler,
        [FromServices] IValidationService<DepositRequest> validationService,
        [FromBody] DepositRequest depositRequest,
        CancellationToken ct)
    {
        validationService.Validate(depositRequest);
        var result = await handler.HandleAsync(depositRequest.ToCommand(), ct);

        return Ok(result);
    }

    [HttpPost("withdrawal")]
    [ProducesResponseType(typeof(Guid), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ValidationErrorResponse), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Withdrawal(
        [FromServices] ICommandHandler<WithdrawalCommand, Guid> handler,
        [FromServices] IValidationService<WithdrawalRequest> validationService,
        [FromBody] WithdrawalRequest withdrawalRequest,
        CancellationToken ct)
    {
        validationService.Validate(withdrawalRequest);

        var result = await handler.HandleAsync(withdrawalRequest.ToCommand(), ct);

        return Ok(result);
    }

    [HttpPost("transfer")]
    [ProducesResponseType(typeof(Guid), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ValidationErrorResponse), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Transfer(
        [FromServices] ICommandHandler<TransferCommand, Guid> handler,
        [FromServices] IValidationService<TransferRequest> validationService,
        [FromBody] TransferRequest transferRequest,
        CancellationToken ct)
    {
        validationService.Validate(transferRequest);

        var result = await handler.HandleAsync(transferRequest.ToCommand(), ct);
        return Ok(result);
    }
}
