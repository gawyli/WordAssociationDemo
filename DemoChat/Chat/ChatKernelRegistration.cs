using Microsoft.Extensions.Configuration;
using Microsoft.SemanticKernel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DemoChat.Common;

namespace DemoChat.Chat;
public static class ChatKernelRegistration
{
    public static IServiceCollection RegisterChatKernel(this IServiceCollection services, IConfiguration configuration)
    {
        // Register Kernel
        var openAICfg = configuration.GetRequiredSection("OpenAI").Get<OpenAIConfig>();
        var sp = services.BuildServiceProvider();

        services.AddKeyedTransient("chat", (sp, key) =>
        {
            var kernelBuilder = Kernel.CreateBuilder();

            kernelBuilder.AddOpenAIChatCompletion(
                openAICfg!.ChatModelId,
                openAICfg!.ApiKey);

            kernelBuilder.AddOpenAITextEmbeddingGeneration(
                openAICfg.EmbeddingModelId,
                openAICfg.ApiKey);

            var kernel = kernelBuilder.Build();

            return kernel;
        });

        return services;
    }
}
