using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.ChatCompletion;

namespace MePoC.Chat;
public interface IChatService
{
    Task StartChatAsync(ChatHistory history, Kernel kernel, CancellationToken cancellationToken);
    void StopChat();
    Task<string> GetAIStreamMessageAsync(ChatHistory history, Kernel kernel, CancellationToken cancellationToken);
    Task<string> GetAIMessageAsync(ChatHistory history, Kernel kernel, CancellationToken cancellationToken);
    Task<string> GetUserMessageAsync(ChatHistory chatHistory, CancellationToken cancellationToken);
}
