using consumer1;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

var settings = builder.Configuration.GetSection(nameof(Settings)).Get<Settings>();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapGet("/products", async () =>
{
    var httpClient = new HttpClient
    {
        BaseAddress = new Uri(settings?.ProviderUrl ?? throw new ArgumentException("Provider Url is required")),
    };

    return await new ProviderService(httpClient).Get();
})
.WithName("GetProducts")
.WithOpenApi();

app.MapGet("/health", () => Results.Ok());

app.Run();
