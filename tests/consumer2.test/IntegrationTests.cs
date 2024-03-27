using System.Net;
using System.Text;
using System.Text.Json;
using Microsoft.AspNetCore.Http;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Moq.Protected;

namespace consumer2.test;

[TestClass]
public class IntegrationTests : TestWebServer<Program>
{
    private readonly HttpClient _httpClient;

    public IntegrationTests()
    {
        _httpClient = CreateClient(); // what is the equivalent of IClassFixture?
    }

    [TestMethod]
    public async Task Get_ProductsExist_ReturnsHttp200()
    {
        var id = 1;
        var product = new Product();
        SetupProviderService(product);

        var response = await _httpClient.GetAsync($"/products/{id}");

        Assert.AreEqual(StatusCodes.Status200OK, (int)response.StatusCode);
    }

    private void SetupProviderService(
        Product product, 
        string httpMethod = "GET", 
        HttpStatusCode httpStatusCode = HttpStatusCode.OK)
    {
        this.HandlerMock
            .Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.Is<HttpRequestMessage>(r => r.Method.Method == httpMethod),
                ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(new HttpResponseMessage
            {
                StatusCode = httpStatusCode,
                Content = GetHttpContent(product),
            });
    }

    private HttpContent GetHttpContent(object content) =>
            new StringContent(
                JsonSerializer.Serialize(content),
                Encoding.UTF8,
                "application/json");
}
