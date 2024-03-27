using Microsoft.AspNetCore.Http;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace consumer1.test;

[TestClass]
public class IntegrationTests : TestWebServer<Program>
{
    private readonly HttpClient _httpClient;

    public IntegrationTests()
    {
        _httpClient = CreateClient();
    }

    [TestMethod]
    public async Task Get_ProductsExist_ReturnsHttp200()
    {
        this.ProviderMock
            .Setup(x => x.Get())
            .ReturnsAsync(
            [
                new() {  Id = 1, Description = "description" }
            ]);

        var response = await _httpClient.GetAsync("/products");

        Assert.AreEqual(StatusCodes.Status200OK, (int)response.StatusCode);
    }
}
