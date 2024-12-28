using DemoChat.Audio.Interfaces;
using DemoChat.Chat.Interfaces;
using DemoChat.Games;
using DemoChat.Games.Interfaces;
using Microsoft.SemanticKernel;

namespace DemoChat;

public class Worker(ILogger<Worker> logger, IServiceProvider serviceProvider) : BackgroundService
{
    private readonly ILogger<Worker> _logger = logger;
    private readonly IServiceProvider _serviceProvider = serviceProvider;

    private async Task InitializeServices(string choice, CancellationToken stoppingToken)
    {
        
        using (var scope = _serviceProvider.CreateScope())
        {
            _logger.LogInformation("Service Provider Scope Created");

            var loggerFactory = scope.ServiceProvider.GetRequiredService<ILoggerFactory>();
            var chatService = scope.ServiceProvider.GetRequiredService<IChatService>();
            var audioService = scope.ServiceProvider.GetRequiredService<IAudioService>();
            
            
            _logger.LogInformation("Services Initialized");

            switch (choice)
            {
                case "1":
                    Console.WriteLine("Playground selected.");

                    var playKernel = scope.ServiceProvider.GetRequiredKeyedService<Kernel>("play");

                    var playgroundLogger = loggerFactory.CreateLogger<Playground>();
                    var playground = new Playground(playgroundLogger, playKernel, chatService, audioService);

                    _logger.LogInformation("Starting Playground...");
                    await playground.StartChat(stoppingToken);

                    break;
                case "2":
                    Console.WriteLine("Word Association Game selected.");
                    var gameService = scope.ServiceProvider.GetRequiredService<IGameService>();
                    var wordKernel = scope.ServiceProvider.GetRequiredKeyedService<Kernel>("word");

                    var gameLogger = loggerFactory.CreateLogger<WordAssociationGame>();
                    var game = new WordAssociationGame(gameLogger, wordKernel, chatService, audioService, gameService);

                    _logger.LogInformation("Starting Game...");
                    await game.StartChat(stoppingToken);

                    break;
                case "3":
                    Console.WriteLine("Exiting...");

                    await StopAsync(stoppingToken);

                    return;
                default:
                    Console.WriteLine("Invalid choice. Please select 1, 2, or 3.");

                    break;
            }
        }
        _logger.LogInformation("Service Provider Scope Closed");
    }



    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        if (_logger.IsEnabled(LogLevel.Information))
        {
            _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
        }

        while (!stoppingToken.IsCancellationRequested)
        {
            Console.WriteLine("Select an option:");
            Console.WriteLine("1. Playground");
            Console.WriteLine("2. Word Association Game");
            Console.WriteLine("3. Exit");

            var choice = Console.ReadLine();
            if (string.IsNullOrEmpty(choice))
                throw new ArgumentNullException("Option cannot be null");

            await InitializeServices(choice, stoppingToken);

            await StopAsync(stoppingToken);
        }
    }

    public override async Task StopAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("Consume Scoped Service Hosted Service is stopping.");

        await base.StopAsync(stoppingToken);
    }
}
