using Microsoft.VisualStudio.TestTools.UnitTesting;
using PactNet;
using PactNet.Verifier;

namespace provider.test;

[TestClass]
public class PactVerificationTests
{
    private readonly ProviderTestServer testServer;

    public PactVerificationTests()
    {
        testServer = new ProviderTestServer();
    }

    [TestMethod]
    public void Verify()
    {
        var config = new PactVerifierConfig
        {
            LogLevel = PactLogLevel.Debug,
        };

        var pactPath = Path.Join(
            "..",
            "..",
            "..",
            "..",
            "..",
            "pacts"); // finds the pact from the consumer.  Needs to be extended for all.

        var verifier = new PactVerifier(config);

        verifier
            .ServiceProvider("ProductsAPI", new Uri(testServer.ServerUrl))
            .WithDirectorySource(new DirectoryInfo(pactPath))
            .WithRequestTimeout(TimeSpan.FromSeconds(3))
            .Verify();
    }
}