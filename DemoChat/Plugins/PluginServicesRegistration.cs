using DemoChat.Games.Interfaces;
using DemoChat.Hume.Interfaces;
using DemoChat.Repository;
using Microsoft.SemanticKernel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DemoChat.Emotions.Interfaces;
using static System.Formats.Asn1.AsnWriter;

namespace DemoChat.Plugins;
public static class PluginServicesRegistration
{
    public static IKernelBuilderPlugins AddWordPlugins(this IKernelBuilderPlugins plugins, IServiceCollection services, IConfiguration configuration)
    {
        var serviceProvider = services.BuildServiceProvider();

        var logger = serviceProvider.GetRequiredService<ILoggerFactory>();
        var gameService = serviceProvider.GetRequiredService<IGameService>();

        plugins.AddFromObject(new WordAssociationPlugin(logger.CreateLogger<WordAssociationPlugin>(), gameService));

        return plugins;
    }

    public static IKernelBuilderPlugins AddPlaygroundPlugin(this IKernelBuilderPlugins plugins, IServiceCollection services, IConfiguration configuration)
    {
        var serviceProvider = services.BuildServiceProvider();

        var logger = serviceProvider.GetRequiredService<ILoggerFactory>();
        var emotionsService = serviceProvider.GetRequiredService<IEmotionsService>();
        var gameService = serviceProvider.GetRequiredService<IGameService>();

        plugins.AddFromObject(new PlaygroundPlugin(logger.CreateLogger<PlaygroundPlugin>(), emotionsService, gameService));

        return plugins;
    }
}
