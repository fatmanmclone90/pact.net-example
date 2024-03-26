using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using producer;

namespace provider.test;
/// <summary>
/// Cannot use WebApplicationFactory, PACT cannot interact with the in memory server
/// https://www.c-sharpcorner.com/article/consumer-driven-contract-testing-using-pactnet/
/// </summary>
public class ProviderTestServer
{
    private readonly IHost server;

#pragma warning disable CA1822 // Mark members as static
    public string ServerUrl => "http://localhost:9050"; // runs API on a known port for PACT
#pragma warning restore CA1822 // Mark members as static

    public ProviderTestServer()
    {
        server = Host
            .CreateDefaultBuilder()
            .ConfigureWebHostDefaults(builder =>
            {
                builder.UseUrls(ServerUrl);
                builder.UseStartup<Startup>();
            })
            .Build();

        server.Start();
    }
}