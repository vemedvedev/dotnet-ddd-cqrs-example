using System.Globalization;
using System.Net.Http.Json;
using System.Net.Mime;
using System.Text;
using Bank.Api.ErrorHandling.ValidationResponse;
using FluentAssertions;
using Newtonsoft.Json.Linq;
using FluentAssertions.Json;

namespace Bank.Api.IntegrationTests.Common.CrudChecker;

public abstract class BaseCrudChecker : CustomWebApplicationFactory<Program>
{
    private HttpClient HttpClient { get; }

    protected BaseCrudChecker(HttpClient httpClient)
    {
        HttpClient = httpClient;
    }

    protected async Task CheckSuccessPost(PostModel postModel)
    {
        var createRequestStr = await File.ReadAllTextAsync(postModel.PathToRawCreateData);

        // Act
        var postResponse = await SendAsJsonAsync(HttpMethod.Post, createRequestStr, postModel.EndpointPost);
        var dataId = await postResponse.Content.ReadFromJsonAsync<long>();

        Assert.NotEqual(0, dataId);
        Assert.Equal(postModel.ExpectedStatusCode, (int)postResponse.StatusCode);

        var requestUri = string.Format(CultureInfo.InvariantCulture, postModel.EndpointGet, dataId);
        var getResponse = await HttpClient.GetAsync(requestUri);
        Assert.Equal(200, (int)getResponse.StatusCode);

        var actualGetDataStr = await getResponse.Content.ReadAsStringAsync();
        var expectedGetDataStr = await File.ReadAllTextAsync(postModel.PathToRawGetData);

        var expectedJToken = JToken.Parse(expectedGetDataStr);
        var actualJToken = JToken.Parse(actualGetDataStr);
        expectedJToken["id"] = dataId;

        actualJToken.Should().ContainSubtree(expectedJToken);
    }

    protected async Task CheckBadRequest(BadRequestValidationModel badRequestValidation)
    {
        var createRequestStr = await File.ReadAllTextAsync(badRequestValidation.PathToRawData);
        var response = await SendAsJsonAsync(badRequestValidation.Method, createRequestStr, badRequestValidation.Endpoint);

        Assert.Equal(400, (int)response.StatusCode);

        var content = await response.Content.ReadAsStringAsync();
        var jToken = JToken.Parse(content);
        var titleString = jToken["title"]!.ToString();
        titleString.Should().Be(ValidationErrorResponse.TitleConst);

        if (badRequestValidation.ExpectedErrors.Any())
        {
            var errorsToken = Assert.IsType<JObject>(jToken["errors"]);

            foreach (var expectedError in badRequestValidation.ExpectedErrors)
            {
                errorsToken.TryGetValue(expectedError.Key, out var actualErrorToken).Should().BeTrue();

                var actualMessages = actualErrorToken!.Values<string>().ToArray();
                foreach (var expectedMessage in expectedError.Value)
                {
                    actualMessages.Should().Contain(expectedMessage);
                }
            }
        }
    }

    private async Task<HttpResponseMessage> SendAsJsonAsync(HttpMethod method, string content, string endpoint)
    {
        var createRequest = new HttpRequestMessage(method, endpoint)
        {
            Content = new StringContent(content, Encoding.UTF8, MediaTypeNames.Application.Json)
        };
        var createResponse = await HttpClient.SendAsync(createRequest);

        return createResponse;
    }
}
