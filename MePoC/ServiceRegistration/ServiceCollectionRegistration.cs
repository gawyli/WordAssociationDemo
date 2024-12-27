using MePoC.Audio;
using MePoC.Audio.Interfaces;
using MePoC.BlobStorage;
using MePoC.Chat;
using MePoC.DataBus;
using MePoC.Hume;
using MePoC.Hume.Interfaces;
using MePoC.Hume.Models;
using MePoC.Repository;
using MePoC.Repository.Interfaces;
using MePoC.WordAssociation;
using MePoC.WordAssociation.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.SemanticKernel.ChatCompletion;
using Refit;
using Serilog;

namespace MePoC.ServiceRegistration;
public static class ServiceCollectionRegistration
{
    public static IServiceCollection RegisterServices(this IServiceCollection services, IConfiguration configuration)
    {
        //var serviceProvider = services.BuildServiceProvider();


        // Logging
        services.RegisterSeliLog(configuration);

        // Blob Storage
        services.AddSingleton<IBlobService, BlobStorageService>();

        // Database
        services.RegisterDatabase(configuration);

        services.AddSingleton<ChatHistory>();

        // Audio
        services.RegisterAudioService(configuration);

        services.AddScoped<IChatService, ChatService>();

        services.RegisterWordService(configuration);

        services.RegisterHumeAI(configuration);

        return services;
    }

    private static IServiceCollection RegisterHumeAI(this IServiceCollection services, IConfiguration configuration)
    {
        var humeAICfg = configuration.GetRequiredSection("HumeAI").Get<HumeAIConfig>();

        services.AddTransient<HumeRequestHandler>();

        services.AddHttpClient("HumeApi", client =>
        {
            client.BaseAddress = new Uri(humeAICfg!.BaseAddress);
            client.DefaultRequestHeaders.Add("X-Hume-Api-Key", humeAICfg.ApiKey);

        });

        services.AddRefitClient<IHumeApi>()
            .ConfigureHttpClient(c => c.BaseAddress = new Uri(humeAICfg!.BaseAddress));

        services.AddScoped<IHumeService, HumeAIService>();

        return services;
    }

    private static IServiceCollection RegisterWordService(this IServiceCollection services, IConfiguration configuration)
    {
        services.RegisterWordKernel(configuration);

        // Word Association Service
        services.AddScoped<IWordAssociationService, WordAssociationTestService>();

        return services;
    }

    private static IServiceCollection RegisterSeliLog(this IServiceCollection services, IConfiguration configuration)
    {
        Log.Logger = new LoggerConfiguration()
            .ReadFrom.Configuration(configuration)
            .Enrich.FromLogContext()
            .CreateLogger();

        services.AddLogging(loggingBuilder => loggingBuilder.AddSerilog(Log.Logger));

        return services;
    }

    private static IServiceCollection RegisterAudioService(this IServiceCollection services, IConfiguration configuration)
    {
        // Kernel
        services.RegisterAudioKernel(configuration);

        services.AddScoped<IAudioService, AudioService>();

        return services;
    }

    private static IServiceCollection RegisterDatabase(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<AppDbContext>(options => options.UseSqlServer(configuration.GetConnectionString("SQLServer")).EnableSensitiveDataLogging());

        services.AddScoped<IRepository, EfRepository>();

        return services;
    }
}
