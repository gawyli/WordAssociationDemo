using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DemoChat.Common;

namespace DemoChat.Chat.Models;
public class ChatSession : BaseEntity
{
    public string? ChatHistory { get; set; }
    public DateTime Created { get; set; }

    public ChatSession(DateTime created)
    {
        this.Created = created;
    }

    // EF Core
    protected ChatSession()
    {
    }

    public void AddChatHistory(string chatHistory)
    {
        this.ChatHistory = chatHistory;
    }
}
