using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DemoChat.Games.Interfaces;
using DemoChat.Games.Models;
using DemoChat.Repository;

namespace DemoChat.Games;
public class GameService : IGameService
{
    private readonly ILogger<GameService> _logger;
    private readonly IRepository _repository;

    public GameService(ILogger<GameService> logger, IRepository repository)
    {
        _logger = logger;
        _repository = repository;
    }

    public async Task<GameSession> CreateGameSession(string chatSessionId, CancellationToken cancellationToken)
    {
        var gameSession = new GameSession(chatSessionId, DateTime.UtcNow);
        return await _repository.AddAsync(gameSession, cancellationToken);
    }

    public async Task AddAssociation(string gameSessionId, string stimulus, string response, CancellationToken cancellationToken)
    {
        var gameSession = await _repository.GetByIdAsync<GameSession>(gameSessionId, cancellationToken);

        if (gameSession != null)
        {
            gameSession.AddAssociation(stimulus, response);

            //Transaction could be useful
            await _repository.UpdateAsync(gameSession, cancellationToken);
        }
        
    }

    public async Task EndGameSession(GameSession gameSession, CancellationToken cancellationToken)
    {
        gameSession.SetEndedNow();

        await _repository.UpdateAsync(gameSession, cancellationToken);
    }

}
