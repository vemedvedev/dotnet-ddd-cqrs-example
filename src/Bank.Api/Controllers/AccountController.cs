using Bank.Api.Contracts.Account.Request;
using Bank.Api.Contracts.Account.Response;
using Bank.Api.ErrorHandling.ValidationResponse;
using Bank.Api.Mappers;
using Bank.Api.Validations;
using Bank.Application.Handlers;
using Bank.Application.Handlers.Account.Create;
using Bank.Application.Handlers.Account.GetAccount;
using Bank.Application.Handlers.Account.GetListAccount;
using Microsoft.AspNetCore.Mvc;

namespace Bank.Api.Controllers;

[Route("[area]/accounts")]
public class AccountController : BaseApiController
{
    [HttpPost]
    [ProducesResponseType(typeof(long), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ValidationErrorResponse), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Create(
        [FromServices] IValidationService<AccountCreateRequest> validationService,
        [FromServices] ICommandHandler<CreateAccountCommand, long> handler,
        [FromBody] AccountCreateRequest createRequest,
        CancellationToken ct)
    {
        validationService.Validate(createRequest);
        var result = await handler.HandleAsync(createRequest.ToCommand(), ct);

        return Ok(result);
    }

    [HttpGet("{id:long}")]
    [ProducesResponseType(typeof(AccountDetailResponse), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetById(
        [FromServices] IQueryHandler<GetAccountQuery, AccountDetailDto> handler,
        [FromRoute] long id,
        CancellationToken ct)
    {
        var result = await handler.HandleAsync(new GetAccountQuery(id), ct);

        return Ok(result.ToDetailResponse());
    }

    [HttpGet]
    [ProducesResponseType(typeof(AccountListResponse), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll(
        [FromServices] IQueryHandler<GetListAccountQuery, ICollection<AccountBriefDto>> handler,
        CancellationToken ct)
    {
        var result = await handler.HandleAsync(new GetListAccountQuery(), ct);

        return Ok(result.Select(x => x.ToBriefResponse()));
    }

}
