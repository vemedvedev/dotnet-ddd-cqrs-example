namespace Bank.Shared.Constants.ErrorCodes;

/// <summary>
/// Code from 200 to 299 reserved for Account related errors.
/// </summary>
public static class AccountBalanceErrors
{
    public const int InsufficientFundsCode = 200;
    public const string InsufficientFundsCodeStr = "insufficient_funds";
    public const string InsufficientFundsUserMessage = "Insufficient funds in the account.";
}
