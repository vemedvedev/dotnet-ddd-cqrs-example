namespace Bank.Domain.Exceptions;

public abstract class DomainRuleBaseException : Exception
{
    protected DomainRuleBaseException(int code, string codeStr, string userMessage, string internalMessage)
        : base(internalMessage)
    {
        Code = code;
        CodeStr = codeStr;
        UserMessage = userMessage;
    }

    protected DomainRuleBaseException(int code, string codeStr, string userMessage)
    {
        Code = code;
        CodeStr = codeStr;
        UserMessage = userMessage;
    }

    public int Code { get; }
    public string CodeStr { get; }
    public string UserMessage { get; }
}
