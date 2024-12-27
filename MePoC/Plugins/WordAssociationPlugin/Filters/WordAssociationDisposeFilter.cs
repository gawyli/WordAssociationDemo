using MePoC.DataBus;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.ChatCompletion;
using System.Text.Json;

namespace MePoC.Plugins.WordAssociationPlugin.Filters;
public class WordAssociationDisposeFilter : IAutoFunctionInvocationFilter
{

    public WordAssociationDisposeFilter()
    {
    }

    public async Task OnAutoFunctionInvocationAsync(AutoFunctionInvocationContext context, Func<AutoFunctionInvocationContext, Task> next)
    {
        var functionName = context.Function.Name;

        await next(context);

        var chatHistory = context.ChatHistory;

        if (functionName == "CompleteWordAssociationTest")
        {
            var message = chatHistory.Where(m => m.Role == AuthorRole.Tool).FirstOrDefault();
            {
                DisposeWordAssociation(chatHistory, message);
            }
        }

    }

    private void DisposeWordAssociation(ChatHistory chatHistory, ChatMessageContent chatMessageContent)
    {
        // Dispose Word Association
        chatHistory.Remove(chatMessageContent);
       //chatHistory.AddAssistantMessage("Word Association Test Completed");
    }
}
