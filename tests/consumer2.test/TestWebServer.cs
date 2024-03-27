using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Moq;

namespace consumer2.test;
/// <summary>
/// Mocks at the HTTP Client Factory level.  Provider Service code is tested, only HTTP traffic is mocked.
/// </summary>
/// <typeparam name="TProgram"></typeparam>
public class TestWebServer<TProgram>
    : WebApplicationFactory<TProgram> where TProgram : class
{
    protected Mock<HttpMessageHandler> HandlerMock { get; set; } = new Mock<HttpMessageHandler>(MockBehavior.Strict);

    protected override IHost CreateHost(IHostBuilder builder)
    {
        builder.ConfigureServices(services =>
        {
            var client = new HttpClient(this.HandlerMock.Object)
            {
                BaseAddress = new Uri("http://productsapi.com"),
            };
            var clientFactoryMock = new Mock<IHttpClientFactory>(MockBehavior.Strict);
            clientFactoryMock
                .Setup(f => f.CreateClient(nameof(IProviderService)))
                .Returns(client);

            services.AddSingleton(clientFactoryMock.Object);
            services.AddSingleton(client);

            // Add or remove any services
            //https://github.com/dotnet/AspNetCore.Docs.Samples/blob/main/fundamentals/minimal-apis/samples/MinApiTestsSample/IntegrationTests/Helpers/TestWebApplicationFactory.cs
        });

        return base.CreateHost(builder);
    }
}