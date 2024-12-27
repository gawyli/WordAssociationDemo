using MePoC;
using MePoC.Logging;
using MePoC.ServiceRegistration;
using Serilog;

SerilogConfig.AddBootstrapLogging();

try
{
    var builder = Host.CreateApplicationBuilder(args);

    var configuration = new ConfigurationBuilder()
        .AddJsonFile("appsettings.json", false)
        .AddUserSecrets<Program>()
        .Build();

    builder.Configuration.AddConfiguration(configuration);

    builder.Services.RegisterServices(configuration);
    builder.Services.AddHostedService<Worker>();

    var host = builder.Build();
    host.Run();
}
catch (Exception ex)
{
    Log.Fatal(ex, "Host terminated unexpectedly");
}
finally
{
    Log.CloseAndFlush();
}

