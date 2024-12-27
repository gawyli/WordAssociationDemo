using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DemoChat.Audio;
using DemoChat.Audio.Interfaces;
using DemoChat.Chat;
using DemoChat.Chat.Interfaces;
using DemoChat.Repository;
using Microsoft.EntityFrameworkCore;

namespace DemoChat;
public static class AppSevicesRegistration
{
    public static IServiceCollection AddServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.RegisterAudioService(configuration);
        services.RegisterChatService(configuration);

        return services;
    }

    public static IServiceCollection AddDatabase(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<AppDbContext>(options => options.UseSqlite(configuration.GetConnectionString("DemoRepository")));

        services.AddScoped<IRepository, EfRepository>();

        return services;
    }

    private static IServiceCollection RegisterAudioService(this IServiceCollection services, IConfiguration configuration)
    {
        // Audio Kernel
        services.RegisterAudioKernel(configuration);

        services.AddScoped<IAudioService, AudioService>();

        return services;
    }

    private static IServiceCollection RegisterChatService(this IServiceCollection services, IConfiguration configuration)
    {
        // Chat Kernels
        services.RegisterPlaygroundKernel(configuration);
        services.RegisterWordAssociationKernel(configuration);

        services.AddScoped<IChatService, ChatService>();

        return services;
    }
}
