// Ignore Spelling: app

using System.Text.Json;

namespace producer;

public class Startup
{
    public void ConfigureServices(IServiceCollection services)
    {
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen();
        services
            .AddControllers()
            .AddJsonOptions(o =>
            {
                o.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
            });
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        if (env.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();

        }

        app.UseRouting();
        app.UseEndpoints(e =>
        {
            e.MapControllers();
        });
    }
}
