namespace producer;

public static class Program
{
    /// <summary>
    /// PACT for the Provider requires a Program and Startup.  Cannot use minimal APIs.
    /// </summary>
    /// <param name="args"></param>
    public static void Main(string[] args)
    {
        CreateHostBuilder(args).Build().Run();
    }

    public static IHostBuilder CreateHostBuilder(string[] args) =>
        Host
            .CreateDefaultBuilder(args)
            .ConfigureWebHostDefaults(webBuilder =>
            {
                webBuilder.ConfigureAppConfiguration((webHostBuilderContext, configurationBuilder) =>
                {
                    configurationBuilder.AddEnvironmentVariables();
                })
                .UseStartup<Startup>();
            });
}