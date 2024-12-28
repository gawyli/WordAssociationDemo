using Microsoft.Extensions.Configuration;
using Microsoft.SemanticKernel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DemoChat.Common;
using DemoChat.Hume.Interfaces;
using DemoChat.Plugins;
using DemoChat.Repository;
using Microsoft.SemanticKernel.ChatCompletion;

namespace DemoChat.Chat;
public static class ChatKernelRegistration
{
    public static IServiceCollection RegisterPlaygroundKernel(this IServiceCollection services, IConfiguration configuration)
    {
        // Register Kernel
        var openAICfg = configuration.GetRequiredSection("OpenAI").Get<OpenAIConfig>();
        var sp = services.BuildServiceProvider();

        services.AddKeyedTransient("play", (sp, key) =>
        {
            var kernelBuilder = Kernel.CreateBuilder();

            kernelBuilder.AddOpenAIChatCompletion(
                openAICfg!.ChatModelId,
                openAICfg!.ApiKey);

            kernelBuilder.AddOpenAITextEmbeddingGeneration(
                openAICfg.EmbeddingModelId,
                openAICfg.ApiKey);

            kernelBuilder.Plugins.AddPlaygroundPlugin(services, configuration);

            var kernel = kernelBuilder.Build();

            return kernel;
        });

        return services;
    }

    public static IServiceCollection RegisterWordAssociationKernel(this IServiceCollection services, IConfiguration configuration)
    {
        // Register Kernel
        var openAICfg = configuration.GetRequiredSection("OpenAI").Get<OpenAIConfig>();
        var sp = services.BuildServiceProvider();

        services.AddKeyedTransient("word", (sp, key) =>
        {
            var kernelBuilder = Kernel.CreateBuilder();

            kernelBuilder.AddOpenAIChatCompletion(
                openAICfg!.ChatModelId,
                openAICfg!.ApiKey);

            kernelBuilder.AddOpenAITextEmbeddingGeneration(
                openAICfg.EmbeddingModelId,
                openAICfg.ApiKey);

            kernelBuilder.Plugins.AddWordPlugins(services, configuration);

            var kernel = kernelBuilder.Build();

            return kernel;
        });

        return services;
    }
}
