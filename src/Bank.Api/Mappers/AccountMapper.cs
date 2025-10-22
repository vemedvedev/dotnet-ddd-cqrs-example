using Bank.Api.Contracts.Account.Request;
using Bank.Api.Contracts.Account.Response;
using Bank.Application.Handlers.Account.Create;
using Bank.Application.Handlers.Account.GetAccount;
using Bank.Application.Handlers.Account.GetListAccount;

namespace Bank.Api.Mappers;

public static class AccountMapper
{
    public static CreateAccountCommand ToCommand(this AccountCreateRequest createRequest) =>
        new()
        {
            Name = createRequest.Name,
            InitialBalance = createRequest.InitialBalance
        };

    public static AccountDetailResponse ToDetailResponse(this AccountDetailDto accountDetailDto) =>
        new()
        {
            Id = accountDetailDto.Id,
            Name = accountDetailDto.Name,
            AccountBalance = accountDetailDto.AccountBalance,
            Uid = accountDetailDto.Uid
        };

    public static AccountBriefResponse ToBriefResponse(this AccountBriefDto accountBriefDto) =>
        new()
        {
            Id = accountBriefDto.Id,
            Name = accountBriefDto.Name,
            AccountBalance = accountBriefDto.AccountBalance,
            Uid = accountBriefDto.Uid
        };
}
