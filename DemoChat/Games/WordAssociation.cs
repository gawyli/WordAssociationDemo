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
using Microsoft.SemanticKernel.ChatCompletion;

namespace DemoChat.Games;
public class WordAssociation
{
    private readonly ILogger<WordAssociation> _logger;
    private readonly IChatService _chatService;
    private readonly IAudioService _audioService;

    private ChatSession _chatSession = null!;
    private ChatHistory _chatHistory = null!;

    private bool _isStopRequest = false;

    public WordAssociation(ILogger<WordAssociation> logger, IChatService chatService, IAudioService audioService)
    {
        _logger = logger;
        _chatService = chatService;
        _audioService = audioService;
    }

    public async Task StartGame(CancellationToken stoppingToken)
    {
        await InitializeChatSession(stoppingToken);
        while (!_isStopRequest)
        {
            Console.WriteLine("\n------------------------");
            await GetUserMessage(stoppingToken);

            if(!_isStopRequest)
                await GetAssistantMessage(stoppingToken);
        }

        if (_isStopRequest)
        {
            await EndChatSession(stoppingToken);
        }
    }

    private async Task InitializeChatSession(CancellationToken cancellationToken)
    {
        var systemPrompt = "You are playing a Word Association Game. Always responds with ONLY one word. If Player say stop, you say goodbye.";

        _chatSession = await _chatService.CreateChatSession(cancellationToken);
        _chatHistory = new ChatHistory(systemPrompt);

    }

    private async Task EndChatSession(CancellationToken cancellationToken)
    {
        var chatHistoryJson = JsonSerializer.Serialize(_chatHistory);
        _chatSession.AddChatHistory(chatHistoryJson);

        await _chatService.SaveChatSession(_chatSession, cancellationToken);
    }

    private async Task GetUserMessage(CancellationToken cancellationToken)
    {
        var record = await _audioService.RecordAudioTest(_chatSession.Id, cancellationToken);
        var userMessage = await _audioService.TranscribeAudio(record, cancellationToken);

        _chatHistory.AddUserMessage(userMessage);

        if (userMessage.StartsWith("stop", StringComparison.OrdinalIgnoreCase))
        {
            _isStopRequest = true;
        }

        Console.WriteLine($"{AuthorRole.User}: \n> {userMessage}\n");
    }

    private async Task GetAssistantMessage(CancellationToken cancellationToken)
    {
        var assistantMessage = await _chatService.SendMessageAsync(_chatHistory, cancellationToken);

        if (!String.IsNullOrEmpty(assistantMessage))
        {
            _chatHistory.AddAssistantMessage(assistantMessage);

            var audioFile = await _audioService!.GenerateAudio(_chatSession.Id, assistantMessage, cancellationToken);
            if (audioFile != null)
            {
                _audioService.PlayAudio(audioFile.Path);
            }

            Console.WriteLine($"{AuthorRole.Assistant}: \n> {assistantMessage}\n");
        }
    }
}
