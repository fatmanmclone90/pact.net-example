using consumer2;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var settings = builder.Configuration.GetSection(nameof(Settings)).Get<Settings>();

builder.Services.AddHttpClient<IProviderService, ProviderService>(o =>
{
    o.BaseAddress = new Uri(settings?.ProviderUrl ?? throw new ArgumentException("Provider Url is required"));
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapGet("/products/{id}", async (int id) =>
{
    using var scope = app.Services.CreateScope();
    var providerService = scope.ServiceProvider.GetRequiredService<IProviderService>();

    return await providerService.Get(id);
})
.WithName("GetProduct")
.WithOpenApi();

app.MapGet("/health", () => Results.Ok());

app.Run();

// allows for integration testing
public partial class Program
{ }
