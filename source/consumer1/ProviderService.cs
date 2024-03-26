using System.Net;
using System.Net.Http.Headers;
using System.Text.Json;

namespace consumer1;

/// <summary>
/// Seems to need to be re-fatored out of program to allow PACT to interact with it in the consumer tests.
/// </summary>
public class ProviderService
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

    public async Task<List<Product>?> Get()
    {
        var response = await httpClient.GetAsync("/products");

        if (response.StatusCode == HttpStatusCode.OK)
        {
            var strContent = await response.Content.ReadAsStringAsync();
            var products = JsonSerializer.Deserialize<List<Product>>(strContent);

            return products;
        }
        else
        {
            throw new HttpRequestException("Unknown response from provider");
        }
    }
}
