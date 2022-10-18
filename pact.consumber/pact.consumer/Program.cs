using pact.consumer;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

var client = new ApiClient(new Uri("http://localhost:6000"));

var products = await client.GetAllProducts();

var product = await client.GetProduct(1);
