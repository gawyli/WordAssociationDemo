using DemoChat.Audio.Interfaces;
using DemoChat.Chat.Interfaces;
using DemoChat.Chat.Models;
using Microsoft.SemanticKernel.ChatCompletion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using DemoChat.Games.Interfaces;
using Microsoft.SemanticKernel;

namespace DemoChat.Games;
public class Playground : IPlayground
{
    private readonly ILogger<Playground> _logger;
    private readonly Kernel _kernel;
    private readonly IChatService _chatService;
    private readonly IAudioService _audioService;

    protected ChatSession _chatSession = null!;
    protected ChatHistory _chatHistory = null!;

    protected bool _isStopRequest = false;

    public Playground(ILogger<Playground> logger, Kernel kernel, IChatService chatService, IAudioService audioService)
    {
        _logger = logger;
        _kernel = kernel;
        _chatService = chatService;
        _audioService = audioService;
    }

    public virtual async Task StartChat(CancellationToken stoppingToken)
    {
        _logger.LogInformation($"Starting chat in {nameof(Playground)}");

        await InitializeChatSession(stoppingToken);
        while (!_isStopRequest)
        {
            Console.WriteLine("\n------------------------");
            await GetUserMessage(stoppingToken);

            if (!_isStopRequest)
                await GetAssistantMessage(stoppingToken);
        }

        if (_isStopRequest)
        {
            await PersistChatSession(stoppingToken);
        }
    }

    protected virtual async Task InitializeChatSession(CancellationToken cancellationToken)
    {
        var systemPrompt = "You are helpful and love small talks. Be a great companion for Human being ";

        _chatSession = await _chatService.CreateChatSession(cancellationToken);
        _chatHistory = new ChatHistory(systemPrompt);

        _chatHistory.AddMessage(AuthorRole.Tool, $"ChatSessionId: {_chatSession.Id}");
    }

    protected async Task GetUserMessage(CancellationToken cancellationToken)
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

    protected async Task GetAssistantMessage(CancellationToken cancellationToken)
    {
        var assistantMessage = await _chatService.SendMessageAsync(_chatHistory, _kernel, cancellationToken);

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

    protected async Task PersistChatSession(CancellationToken cancellationToken)
    {
        var chatHistoryJson = JsonSerializer.Serialize(_chatHistory);
        _chatSession.AddChatHistory(chatHistoryJson);

        await _chatService.SaveChatSession(_chatSession, cancellationToken);
    }

}
