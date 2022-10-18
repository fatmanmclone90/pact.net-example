namespace pact.consumer.test;

using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using PactNet;
using PactNet.Matchers;
using Shouldly;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using Xunit;
using Xunit.Abstractions;

public class ApiTest
{
    private readonly IPactBuilderV3 pact;
    private readonly ApiClient ApiClient;
    private readonly int port = 9000;
    private readonly List<object> _products;

    public ApiTest(ITestOutputHelper output)
    {
        _products = new List<object>()
            {
                new { id = 9, type = "CREDIT_CARD", name = "GEM Visa", version = "v2" },
                new { id = 10, type = "CREDIT_CARD", name = "28 Degrees", version = "v1" }
            };

        var Config = new PactConfig
        {
            PactDir = Path.Join("..", "..", "..", "..", "..", "pacts"),
            Outputters = new[] { new XUnitOutput(output) },
            DefaultJsonSettings = new JsonSerializerSettings
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver()
            }
        };

        pact = Pact.V3("ApiClient", "ProductService", Config).WithHttpInteractions(port);
        ApiClient = new ApiClient(new Uri($"http://localhost:{port}"));
    }

    [Fact]
    public async Task GetAllProducts()
    {
        // Arange
        pact.UponReceiving("A valid request for all products")
                .Given("There is data")
                .WithRequest(HttpMethod.Get, "/api/products")
            .WillRespond()
                .WithStatus(HttpStatusCode.OK)
                .WithHeader("Content-Type", "application/json; charset=utf-8")
                .WithJsonBody(new TypeMatcher(_products));

        await pact.VerifyAsync(async ctx =>
        {
            var products = await ApiClient.GetAllProducts();

            products.Count().ShouldBe(2);
            var product1 = products.Single(x => x.Id == 9);
            product1.Type.ShouldBe("CREDIT_CARD");
        });
    }

    [Fact]
    public async Task GetProduct()
    {
        // Arange
        pact.UponReceiving("A valid request for a product")
                .Given("There is data")
                .WithRequest(HttpMethod.Get, "/api/product/10")
            .WillRespond()
                .WithStatus(HttpStatusCode.OK)
                .WithHeader("Content-Type", "application/json; charset=utf-8")
                .WithJsonBody(new TypeMatcher(_products[1]));

        await pact.VerifyAsync(async ctx =>
        {
            var response = await ApiClient.GetProduct(10);
        });
    }
}