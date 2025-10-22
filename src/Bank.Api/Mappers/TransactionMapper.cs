using Bank.Api.Contracts.Transaction.Request;
using Bank.Application.Handlers.Transaction.Deposit;
using Bank.Application.Handlers.Transaction.Withdrawal;
using Bank.Application.Handlers.Transaction.Transfer;

namespace Bank.Api.Mappers;

public static class TransactionMapper
{
    public static DepositCommand ToCommand(this DepositRequest depositRequest) =>
        new()
        {
            AccountId = depositRequest.AccountId,
            Amount = depositRequest.Amount
        };

    public static WithdrawalCommand ToCommand(this WithdrawalRequest withdrawalRequest) =>
        new()
        {
            AccountId = withdrawalRequest.AccountId,
            Amount = withdrawalRequest.Amount
        };

    public static TransferCommand ToCommand(this TransferRequest transferRequest) =>
        new()
        {
            FromAccountId = transferRequest.FromAccountId,
            ToAccountId = transferRequest.ToAccountId,
            Amount = transferRequest.Amount
        };
}
