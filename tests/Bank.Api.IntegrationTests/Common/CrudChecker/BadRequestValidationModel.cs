namespace Bank.Api.IntegrationTests.Common.CrudChecker;

public record BadRequestValidationModel
{
    public required string Endpoint { get; init; } = string.Empty;
    public required string PathToRawData { get; init; } = string.Empty;
    public required HttpMethod Method { get; init; }
    public required IReadOnlyDictionary<string, string[]> ExpectedErrors { get; init; }
}
