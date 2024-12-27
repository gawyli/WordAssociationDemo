using Microsoft.SemanticKernel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DemoChat.Common;

namespace DemoChat.Audio;
public static class AudioKernelRegistration
{
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
}
