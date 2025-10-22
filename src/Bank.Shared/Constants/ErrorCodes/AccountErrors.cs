namespace Bank.Shared.Constants.ErrorCodes;

/// <summary>
/// Code from 100 to 199 reserved for Account related errors.
/// </summary>
public static class AccountErrors
{
    public const int NameAlreadyUsedCode = 100;
    public const string NameAlreadyUsedCodeStr = "account_name_already_used";
    public const string NameAlreadyExistsUserMessage = "Account name is already used.";

    public const int AccountNotFoundCode = 101;
    public const string AccountNotFoundCodeStr = "account_not_found";
    public const string AccountNotFoundUserMessage = "Account not found.";
}
