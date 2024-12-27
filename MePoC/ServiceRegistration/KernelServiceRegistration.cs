using MePoC.DataBus;
using MePoC.Hume.Interfaces;
using MePoC.Plugins.HumePlugin;
using MePoC.Plugins.WordAssociationPlugin;
using MePoC.Plugins.WordAssociationPlugin.Filters;
using MePoC.Repository.Interfaces;
using MePoC.ServiceRegistration.Models;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.ChatCompletion;

namespace MePoC.ServiceRegistration;
public static class KernelServiceRegistration
{
    public static IServiceCollection RegisterWordKernel(this IServiceCollection services, IConfiguration configuration)
    {
        // Register Kernel
        var openAICfg = configuration.GetRequiredSection("OpenAI").Get<OpenAIConfig>();

        var sp = services.BuildServiceProvider();


        services.AddKeyedTransient("G4T", (sp, key) =>
        {
            var kernelBuilder = Kernel.CreateBuilder();

            // Alternative using OpenAI
            kernelBuilder.AddOpenAIChatCompletion(
                openAICfg!.WordModelId,
                openAICfg!.ApiKey);

            kernelBuilder.AddOpenAITextEmbeddingGeneration(
                openAICfg.EmbeddingModelId,
                openAICfg.ApiKey);

            //kernelBuilder.Services.AddSingleton<IAutoFunctionInvocationFilter>(new WordAssociationDisposeFilter());

            kernelBuilder.Plugins.AddPlugins(services, configuration);

            var kernel = kernelBuilder.Build();

            return kernel;
        });

        return services;
    }
    public static IServiceCollection RegisterAudioKernel(this IServiceCollection services, IConfiguration configuration)
    {
        // Register Kernel
        var openAICfg = configuration.GetRequiredSection("OpenAI").Get<OpenAIConfig>();
        var sp = services.BuildServiceProvider();

        services.AddKeyedTransient("audio", (sp, key) =>
        {
            var kernelBuilder = Kernel.CreateBuilder();

            kernelBuilder.AddOpenAIAudioToText(
                openAICfg!.AudioToTextModelId,
                openAICfg!.ApiKey);

            kernelBuilder.AddOpenAITextToAudio(
                openAICfg.TextToAudioModelId,
                openAICfg.ApiKey);

            var kernel = kernelBuilder.Build();

            return kernel;
        });

        return services;
    }

    private static IKernelBuilderPlugins AddPlugins(this IKernelBuilderPlugins plugins, IServiceCollection services, IConfiguration configuration)
    {
        var serviceProvider = services.BuildServiceProvider();

        var logger = serviceProvider.GetRequiredService<ILoggerFactory>();
        var repository = serviceProvider.GetRequiredService<IRepository>();
        var humeAI = serviceProvider.GetRequiredService<IHumeService>();
        var chatHistory = serviceProvider.GetRequiredService<ChatHistory>();

        //plugins.AddFromObject(new HumeAIPlugin(logger.CreateLogger<HumeAIPlugin>(), humeAI, repository));
        plugins.AddFromObject(new WordAssociationTestPlugin(logger.CreateLogger<WordAssociationTestPlugin>(), repository, chatHistory));

        return plugins;
    }
}
