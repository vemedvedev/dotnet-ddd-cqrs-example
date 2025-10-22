namespace Bank.Api.IntegrationTests.Common.CrudChecker;

public record PostModel
{
    public required string EndpointPost { get; init; } = string.Empty;
    public required string EndpointGet { get; init; } = string.Empty;
    public required string PathToRawCreateData { get; init; } = string.Empty;
    public required string PathToRawGetData { get; init; } = string.Empty;
    public required long ExpectedStatusCode { get; init; }
}
