using DemoChat.Games.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DemoChat.Games.Interfaces;
public interface IGameService
{
    Task<List<string>> GetGameSessions(CancellationToken cancellationToken);
    Task<GameSession> CreateGameSession(string chatSessionId, CancellationToken cancellationToken);
    Task AddAssociation(string gameSessionId, string stimulus, string response, CancellationToken cancellationToken);
    Task SetEmotionsSessionId(string gameSessionId, string jobInferenceId, CancellationToken cancellationToken);
    Task EndGameSession(GameSession gameSession, CancellationToken cancellationToken);

}
