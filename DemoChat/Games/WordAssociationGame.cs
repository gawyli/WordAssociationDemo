using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using DemoChat.Audio.Interfaces;
using DemoChat.Chat.Interfaces;
using DemoChat.Chat.Models;
using DemoChat.Games.Interfaces;
using DemoChat.Games.Models;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.ChatCompletion;

namespace DemoChat.Games;
public class WordAssociationGame : Playground
{
    private readonly ILogger<WordAssociationGame> _logger;
    private readonly IChatService _chatService;
    private readonly IGameService _gameService;

    private GameSession _gameSession;

    public WordAssociationGame(ILogger<WordAssociationGame> logger, Kernel kernel, IChatService chatService, IAudioService audioService, IGameService gameService) : base(logger, kernel, chatService, audioService)
    {
        _logger = logger;
        _chatService = chatService;
        _gameService = gameService;
    }
    
    // In game the AI starts first with stimulus word
    public override async Task StartChat(CancellationToken stoppingToken)
    {
        _logger.LogInformation($"Starting chat in {nameof(WordAssociationGame)}");

        await InitializeChatSession(stoppingToken);
        await InitializeGameSession(stoppingToken);

        while (!base._isStopRequest)
        {
            Console.WriteLine("\n------------------------");
            
            await base.GetAssistantMessage(stoppingToken);
            await base.GetUserMessage(stoppingToken);
        }

        if (base._isStopRequest)
        {
            await base.PersistChatSession(stoppingToken);
            await _gameService.EndGameSession(_gameSession, stoppingToken);
        }
    }

    protected override async Task InitializeChatSession(CancellationToken cancellationToken)
    {
        var systemPrompt = 
"""
You are playing a Word Association Game. 
Always responds with ONLY one stimulus word. 
Player can ONLY have one response word.

Association is a stimulus word and response word.
 
Association example
apple
fruit

For each association (stimulus word and response word) you MUST call function to add association to the database.
""";

        _chatSession = await _chatService.CreateChatSession(cancellationToken);
        _chatHistory = new ChatHistory(systemPrompt);

    }

    private async Task InitializeGameSession(CancellationToken cancellationToken)
    {
        _gameSession = await _gameService.CreateGameSession(_chatSession.Id, cancellationToken);

        _chatSession.AddGameSession(_gameSession);
        await _chatService.SaveChatSession(_chatSession, cancellationToken);

        _chatHistory.AddMessage(AuthorRole.System, $"GameSessionId: {_gameSession.Id}");
    }
}
