using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DemoChat.Audio.Interfaces;
using DemoChat.Chat.Interfaces;
using DemoChat.Chat.Models;
using DemoChat.Games.Models;
using DemoChat.Repository;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.ChatCompletion;
using Microsoft.SemanticKernel.Connectors.OpenAI;

namespace DemoChat.Chat;
public class ChatService : IChatService
{
    private readonly ILogger<ChatService> _logger;
    private readonly IRepository _repository;

    public ChatService(ILogger<ChatService> logger, IRepository repository)
    {
        _logger = logger;
        _repository = repository;
    }

    public async Task<List<KeyValuePair<string, string>>> GetChatSession(CancellationToken cancellationToken)
    {
        // Specifications would be nice
        var chatSessions = await _repository.ListAsync<ChatSession>(cancellationToken);
        var ids = chatSessions.Select(gs => new KeyValuePair<string, string>(gs.Id, gs.Created.ToString("yyyy-MM-ddTHH:mm:ss"))).ToList();
        return ids;
    }

    public async Task<ChatSession> CreateChatSession(CancellationToken cancellationToken)
    {
        var chatSession = await _repository.AddAsync(new ChatSession(DateTime.UtcNow), cancellationToken);
        _logger.LogInformation($"Chat Session {chatSession.Id} created");

        return chatSession;
    }

    public async Task<string> SendMessageAsync(ChatHistory chatHistory, Kernel kernel, CancellationToken cancellationToken)
    {
        OpenAIPromptExecutionSettings openAIPromptExecutionSettings = new()
        {
            ToolCallBehavior = ToolCallBehavior.AutoInvokeKernelFunctions
        };

        var chat = kernel.Services.GetRequiredService<IChatCompletionService>();
        var responseContent = await chat.GetChatMessageContentAsync(chatHistory, openAIPromptExecutionSettings, kernel, cancellationToken);

        var response = responseContent.Content ?? string.Empty;

        return response;
    }

    public async Task SaveChatSession(ChatSession chatSession, CancellationToken cancellationToken)
    {
        await _repository.UpdateAsync(chatSession, cancellationToken);
        _logger.LogInformation($"Chat Session {chatSession.Id} updated");
    }
}
