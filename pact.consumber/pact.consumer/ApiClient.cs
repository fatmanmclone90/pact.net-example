namespace pact.consumer
{
    public class ApiClient
    {
        private readonly Uri BaseUri;

        public ApiClient(Uri baseUri)
        {
            this.BaseUri = baseUri;
        }

        public async Task<IEnumerable<Product>?> GetAllProducts()
        {
            using (var client = new HttpClient { BaseAddress = BaseUri })
            {
                var response = await client.GetAsync($"/api/products");
                return await response.Content.ReadFromJsonAsync<IEnumerable<Product>>();
            }
        }

        public async Task<Product?> GetProduct(int id)
        {
            using (var client = new HttpClient { BaseAddress = BaseUri })
            {
                var response = await client.GetAsync($"/api/product/{id}");
                return await response.Content.ReadFromJsonAsync<Product>();
            }
        }
    }
}
