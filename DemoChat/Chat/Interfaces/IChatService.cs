using Microsoft.SemanticKernel.ChatCompletion;
using Microsoft.SemanticKernel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DemoChat.Chat.Models;

namespace DemoChat.Chat.Interfaces;
public interface IChatService
{
    Task<ChatSession> CreateChatSession(CancellationToken cancellationToken);
    Task<string> SendMessageAsync(ChatHistory history, CancellationToken cancellationToken);
    Task SaveChatSession(ChatSession chatSession, CancellationToken cancellationToken);
}
