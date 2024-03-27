using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Moq;

namespace consumer1.test;
/// <summary>
/// Mocks at the Client level, Provider Service is not tested by these integration tests
/// </summary>
/// <typeparam name="TProgram"></typeparam>
public class TestWebServer<TProgram>
    : WebApplicationFactory<TProgram> where TProgram : class
{
    public Mock<IProviderService> ProviderMock = new(MockBehavior.Strict);

    protected override IHost CreateHost(IHostBuilder builder)
    {
        builder.ConfigureServices(services =>
        {
            // no need to remove first
            services.AddScoped(sp => ProviderMock.Object);

            // Add or remove any services
            //https://github.com/dotnet/AspNetCore.Docs.Samples/blob/main/fundamentals/minimal-apis/samples/MinApiTestsSample/IntegrationTests/Helpers/TestWebApplicationFactory.cs
        });

        return base.CreateHost(builder);
    }
}