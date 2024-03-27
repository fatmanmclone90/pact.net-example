using System.Net;
using System.Net.Http.Headers;
using System.Text.Json;

namespace consumer2;

/// <summary>
/// Seems to need to be re-factored out of program to allow PACT to interact with it in the consumer tests.
/// </summary>
public class ProviderService : IProviderService
{
    private readonly HttpClient httpClient;

    public ProviderService(
        HttpClient httpClient)
    {
        this.httpClient = httpClient;
        this.httpClient.DefaultRequestHeaders
          .Accept
          .Add(new MediaTypeWithQualityHeaderValue("application/json"));

    }

    public async Task<Product?> Get(int id)
    {
        var response = await httpClient.GetAsync($"/products/{id}");

        if (response.StatusCode == HttpStatusCode.OK)
        {
            var strContent = await response.Content.ReadAsStringAsync();
            var product = JsonSerializer.Deserialize<Product>(strContent);

            return product;
        }
        else
        {
            throw new HttpRequestException("Unknown response from provider");
        }
    }
}
