using MePoC.Audio.Interfaces;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.ChatCompletion;
using Microsoft.SemanticKernel.Connectors.OpenAI;

namespace MePoC.Chat;
public class ChatService : IChatService
{
    private readonly ILogger<ChatService> _logger;
    private readonly IAudioService _audioService;

    public bool IsStopRequested { get; set; } = false;

    public ChatService(ILogger<ChatService> logger, IAudioService audioService)
    {
        _logger = logger;
        _audioService = audioService;
    }

    public async Task StartChatAsync(ChatHistory history, Kernel kernel, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Starting Chat");

        Console.WriteLine($"Press enter to speak");
        // Loop till we are cancelled
        while (!IsStopRequested)
        {
            // Get User Message
            var message = await GetUserMessageAsync(history, cancellationToken);
            if (message.Contains("stop chat", StringComparison.OrdinalIgnoreCase))
            {
                StopChat();
            }

            // Get AI response
            await GetAIStreamMessageAsync(history, kernel, cancellationToken);

        }
    }

    public void StopChat()
    {
        IsStopRequested = true;
    }

    public async Task<string> GetAIStreamMessageAsync(ChatHistory history, Kernel kernel, CancellationToken cancellationToken)
    {
        bool roleWritten = false;
        string fullMessage = string.Empty;

        var chat = kernel.Services.GetRequiredService<IChatCompletionService>();

        OpenAIPromptExecutionSettings openAIPromptExecutionSettings = new()
        {
            ToolCallBehavior = ToolCallBehavior.AutoInvokeKernelFunctions
        };

        _logger.LogInformation("Getting Stream Chat");
        await foreach (var chatUpdate in chat.GetStreamingChatMessageContentsAsync(history, openAIPromptExecutionSettings, kernel, cancellationToken))
        {
            if (!roleWritten && chatUpdate.Role.HasValue)
            {
                Console.WriteLine($"{chatUpdate.Role.Value}: \n> {chatUpdate.Content}\n");
                roleWritten = true;
            }

            if (chatUpdate.Content is { Length: > 0 })
            {
                fullMessage += chatUpdate.Content;
                Console.Write(chatUpdate.Content);
            }
        }

        var response = await _audioService!.GenerateAudio(fullMessage, cancellationToken);
        if (response != null)
        {
            _audioService.PlayAudio(response.AudioPath);
        }

        Console.WriteLine("\n------------------------");
        history.AddMessage(AuthorRole.Assistant, fullMessage);

        return fullMessage;
    }

    public async Task<string> GetAIMessageAsync(ChatHistory chatHistory, Kernel kernel, CancellationToken cancellationToken)
    {
        OpenAIPromptExecutionSettings openAIPromptExecutionSettings = new()
        {
            ToolCallBehavior = ToolCallBehavior.AutoInvokeKernelFunctions
        };

        var chat = kernel.Services.GetRequiredService<IChatCompletionService>();
        var aiResponseContent = await chat.GetChatMessageContentAsync(chatHistory, openAIPromptExecutionSettings, kernel, cancellationToken);

        var aiResponse = aiResponseContent.Content ?? string.Empty;      

        if (!String.IsNullOrEmpty(aiResponse))
        {
            Console.WriteLine($"{AuthorRole.Assistant}: \n> {aiResponse}\n");


            var response = await _audioService!.GenerateAudio(aiResponse, cancellationToken);
            if (response != null)
            {
                _audioService.PlayAudio(response.AudioPath);
            }

            Console.WriteLine("\n------------------------");
            chatHistory.AddMessage(AuthorRole.Assistant, aiResponse);
        }

        return aiResponse;
    }

    public async Task<string> GetUserMessageAsync(ChatHistory chatHistory, CancellationToken cancellationToken)
    { 
        // Get user input
        var userInput = Console.ReadLine();


        var record = await _audioService!.RecordAudioTest(cancellationToken);
        var userMessage = await _audioService!.TranscribeAudio(record, cancellationToken);

       
        Console.WriteLine($"{AuthorRole.User}:");
        Console.WriteLine("> " + userMessage);
        chatHistory.AddUserMessage(userMessage);

        return userMessage;
    }
}
