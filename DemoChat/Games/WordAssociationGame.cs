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
using DemoChat.Games.Models;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.ChatCompletion;

namespace DemoChat.Games;
public class WordAssociationGame : Playground
{
    private readonly ILogger<WordAssociationGame> _logger;
    private readonly IChatService _chatService;

    public WordAssociationGame(ILogger<WordAssociationGame> logger, Kernel kernel, IChatService chatService, IAudioService audioService) : base(logger, kernel, chatService, audioService)
    {
        _logger = logger;
        _chatService = chatService;
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
        }
    }

    protected override async Task InitializeChatSession(CancellationToken cancellationToken)
    {
        var systemPrompt = "You are playing a Word Association Game. Always responds with ONLY one stimulus word. Player can ONLY have one response word. Example\napple\nFruit\n\n For every stimulus and response word you MUST call functions to persists words in database.";

        _chatSession = await _chatService.CreateChatSession(cancellationToken);
        _chatHistory = new ChatHistory(systemPrompt);
    }

    private async Task InitializeGameSession(CancellationToken cancellationToken)
    {
        var wordAssociationGameSession = new WordAssociation(_chatSession.Id, DateTime.UtcNow);

        var session = await _chatService.PersistSession(wordAssociationGameSession, cancellationToken);

        _chatSession.WordAssociationId = session.Id;

        _chatHistory.AddMessage(AuthorRole.System, $"GameSessionId: {session.Id}");
    }

}
