using System.Net;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using PactNet;

namespace consumer1.test;

[TestClass]
public class PactVerificationTests
{
    private readonly IPactBuilderV3 pactBuilder;

    public PactVerificationTests()
    {
        var pact = Pact.V3(
            "consumer1",
            "ProductsAPI",
            new PactConfig
            {
                LogLevel = PactLogLevel.Information,
                PactDir = $"{Directory.GetParent(Directory.GetCurrentDirectory())?.Parent?.Parent?.Parent?.Parent?.FullName}{Path.DirectorySeparatorChar}pacts" ?? throw new ArgumentException("Unknown pact location"), // places pacts in root
            });
        pactBuilder = pact.WithHttpInteractions();
    }

    [TestMethod]
    public async Task TestMethod1()
    {
        pactBuilder
            .UponReceiving("A GET request to /products")
                .Given("There are products available")
                .WithRequest(HttpMethod.Get, "/products")
                .WithHeader("Accept", "application/json")
            .WillRespond()
                .WithStatus(HttpStatusCode.OK)
                .WithHeader("Content-Type", "application/json; charset=utf-8")
                .WithJsonBody(
                    new List<Product>
                    {
                        new()
                        {
                            Id = 1,
                            Description = "fork",
                        }
                    }, 
                    new JsonSerializerSettings 
                    { 
                        ContractResolver = new CamelCasePropertyNamesContractResolver() // pact uses newtonsoft and is case sensitive on comparision
                    }
                );

        await pactBuilder.VerifyAsync(async ctx =>
        {
            var httpClient = new HttpClient
            {
                BaseAddress = ctx.MockServerUri
            };

            var provider = new ProviderService(httpClient);
            var response = await provider.Get();
            
            Assert.IsNotNull(response);
        });
    }
}