using DemoChat.Hume.Interfaces;
using DemoChat.Repository;
using Microsoft.SemanticKernel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DemoChat.Plugins;
public static class PluginServicesRegistration
{
    public static IKernelBuilderPlugins AddWordPlugins(this IKernelBuilderPlugins plugins, IServiceCollection services, IConfiguration configuration)
    {
        var serviceProvider = services.BuildServiceProvider();

        var logger = serviceProvider.GetRequiredService<ILoggerFactory>();
        var repository = serviceProvider.GetRequiredService<IRepository>();
        //var humeAI = serviceProvider.GetRequiredService<IHumeService>();
        //var chatHistory = serviceProvider.GetRequiredService<ChatHistory>();

        //plugins.AddFromObject(new HumeAIPlugin(logger.CreateLogger<HumeAIPlugin>(), humeAI, repository));
        plugins.AddFromObject(new WordAssociationPlugin(logger.CreateLogger<WordAssociationPlugin>(), repository));

        return plugins;
    }
}
