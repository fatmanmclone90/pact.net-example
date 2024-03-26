using System.Net;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using PactNet;
using PactNet.Matchers;

namespace consumer2.test;

[TestClass]
public class PactVerificationTests
{
    private readonly IPactBuilderV3 pactBuilder;

    public PactVerificationTests()
    {
        var pact = Pact.V3(
            "consumer2",
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
        var productId = 10;
        pactBuilder
            .UponReceiving("A GET request to /products/:id")
                .Given("There are products available")
                .WithRequest(HttpMethod.Get, $"/products/{productId}")
                .WithHeader("Accept", "application/json")
            .WillRespond()
                .WithStatus(HttpStatusCode.OK)
                .WithHeader("Content-Type", "application/json; charset=utf-8")
                // using dynamic type allows for more flexible matching but moves away from using the POCO
                .WithJsonBody(new
                {
                    Id = Match.Type(123),
                    Description = Match.Type("fork")
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
            var response = await provider.Get(productId);

            Assert.IsNotNull(response);
        });
    }
}