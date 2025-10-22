using System.Diagnostics.CodeAnalysis;
using Bank.Api.IntegrationTests.Common.CrudChecker;

namespace Bank.Api.IntegrationTests.Account;

[SuppressMessage("Naming", "CA1707:Identifiers should not contain underscores")]
public class AccountControllerTests : BaseCrudChecker,
    IClassFixture<CustomWebApplicationFactory<Program>>
{
    public AccountControllerTests(CustomWebApplicationFactory<Program> factory) : base(factory.CreateClient())
    {
    }

    [Fact]
    public async Task Create_AccountWithInitialBalance_ReturnExpectedContract() => await CheckSuccessPost(
        new PostModel
        {
            ExpectedStatusCode = 200,
            EndpointPost = "/api/accounts",
            EndpointGet = "/api/accounts/{0}",
            PathToRawCreateData = "Account/Data/Create/Success/account_create_full.json",
            PathToRawGetData = "Account/Data/Create/Success/account_create_full_get.json"
        });

    [Fact]
    public async Task Create_AccountMinimumData_ReturnExpectedContract()
        => await CheckSuccessPost(
            new PostModel
            {
                ExpectedStatusCode = 200,
                EndpointPost = "/api/accounts",
                EndpointGet = "/api/accounts/{0}",
                PathToRawCreateData = "Account/Data/Create/Success/account_create_minimum.json",
                PathToRawGetData = "Account/Data/Create/Success/account_create_minimum_get.json"
            });

    [Fact]
    public async Task Create_AccountWithoutName_ReturnsValidationError()
        => await CheckBadRequest(
            new BadRequestValidationModel
            {
                Endpoint = "/api/accounts",
                Method = HttpMethod.Post,
                PathToRawData = "Account/Data/Create/Errors/account_create_without_name.json",
                ExpectedErrors = new Dictionary<string, string[]>
                {
                    ["Name"] = ["'Name' must not be empty."]
                }
            });
}
