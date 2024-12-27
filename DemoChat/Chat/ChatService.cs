using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DemoChat.Audio.Interfaces;
using DemoChat.Chat.Interfaces;
using DemoChat.Chat.Models;
using DemoChat.Repository;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.ChatCompletion;
using Microsoft.SemanticKernel.Connectors.OpenAI;

namespace DemoChat.Chat;
public class ChatService : IChatService
{
    private readonly ILogger<ChatService> _logger;
    private readonly Kernel _kernel;
    private readonly IRepository _repository;

    public ChatService(ILogger<ChatService> logger, [FromKeyedServices("chat")] Kernel kernel, IRepository repository)
    {
        _logger = logger;
        _kernel = kernel;
        _repository = repository;
    }

    public async Task<ChatSession> CreateChatSession(CancellationToken cancellationToken)
    {
        var chatSession = await _repository.AddAsync(new ChatSession(DateTime.UtcNow), cancellationToken);
        _logger.LogInformation($"Chat Session {chatSession.Id} created");

        return chatSession;
    }

    public async Task<string> SendMessageAsync(ChatHistory chatHistory, CancellationToken cancellationToken)
    {
        OpenAIPromptExecutionSettings openAIPromptExecutionSettings = new()
        {
            ToolCallBehavior = ToolCallBehavior.AutoInvokeKernelFunctions
        };

        var chat = _kernel.Services.GetRequiredService<IChatCompletionService>();
        var responseContent = await chat.GetChatMessageContentAsync(chatHistory, openAIPromptExecutionSettings, _kernel, cancellationToken);

        var response = responseContent.Content ?? string.Empty;

        return response;
    }

    public async Task SaveChatSession(ChatSession chatSession, CancellationToken cancellationToken)
    {
        await _repository.UpdateAsync(chatSession, cancellationToken);
        _logger.LogInformation($"Chat Session {chatSession.Id} updated");
    }
}
