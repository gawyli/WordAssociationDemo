using DemoChat.Audio.Interfaces;
using DemoChat.Chat.Interfaces;
using DemoChat.Games;

namespace DemoChat;

public class Worker(ILogger<Worker> logger, IServiceProvider serviceProvider) : BackgroundService
{
    private readonly ILogger<Worker> _logger = logger;
    private readonly IServiceProvider _serviceProvider = serviceProvider;

    private async Task InitializeServices(CancellationToken stoppingToken)
    {
        using (var scope = _serviceProvider.CreateScope())
        {
            var loggerFactory = scope.ServiceProvider.GetRequiredService<ILoggerFactory>();
            var chatService = scope.ServiceProvider.GetRequiredService<IChatService>();
            var audioService = scope.ServiceProvider.GetRequiredService<IAudioService>();
            _logger.LogInformation("Services Initialized");

            var logger = loggerFactory.CreateLogger<WordAssociation>();
            var game = new WordAssociation(logger, chatService, audioService);

            _logger.LogInformation("Starting Game...");
            await game.StartGame(stoppingToken);
        }
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        if (_logger.IsEnabled(LogLevel.Information))
        {
            _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
            await InitializeServices(stoppingToken);
        }

        await StopAsync(stoppingToken);
    }

    public override async Task StopAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("Consume Scoped Service Hosted Service is stopping.");

        await base.StopAsync(stoppingToken);
    }
}
