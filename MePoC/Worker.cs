using MePoC.WordAssociation.Interfaces;

namespace MePoC;

public class Worker : BackgroundService
{
    private readonly ILogger<Worker> _logger;
    private readonly IServiceProvider _serviceProvider;

    private IWordAssociationService _wordService = null!;

    public Worker(ILogger<Worker> logger, IServiceProvider serviceProvider)
    {
        _logger = logger;
        _serviceProvider = serviceProvider;
    }

    private async Task InitializeServices(CancellationToken stoppingToken)
    {
        using (var scope = _serviceProvider.CreateScope())
        {
            _wordService = scope.ServiceProvider.GetRequiredService<IWordAssociationService>();
            _logger.LogInformation("Chat Service Initialized");

            _logger.LogInformation("Run chat service");
            await _wordService.InitializeChat(stoppingToken);
        }

        await StopAsync(stoppingToken);
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        if (_logger.IsEnabled(LogLevel.Information))
        {
            _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
            await InitializeServices(stoppingToken);
        }
    }

    public override async Task StopAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("Consume Scoped Service Hosted Service is stopping.");

        await base.StopAsync(stoppingToken);
    }
}
