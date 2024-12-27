using Microsoft.SemanticKernel.ChatCompletion;
using Microsoft.SemanticKernel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DemoChat.Chat.Models;
using DemoChat.Games.Models;

namespace DemoChat.Chat.Interfaces;
public interface IChatService
{
    Task<ChatSession> CreateChatSession(CancellationToken cancellationToken);
    Task<string> SendMessageAsync(ChatHistory history, Kernel kernel, CancellationToken cancellationToken);
    Task SaveChatSession(ChatSession chatSession, CancellationToken cancellationToken);
    Task<WordAssociation> PersistSession(WordAssociation wordAssociationSession, CancellationToken cancellationToken);
}
