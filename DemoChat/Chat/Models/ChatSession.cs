using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DemoChat.Audio.Models;
using DemoChat.Common;
using DemoChat.Games;
using DemoChat.Games.Models;

namespace DemoChat.Chat.Models;
public class ChatSession : BaseEntity
{
    public string? ChatHistory { get; set; }
    public DateTime Created { get; set; }
    public List<AudioFile> AudioFiles { get; set; } = new();
    public List<GameSession> GameSessions { get; set; } = new();

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

    public void AddGameSession(GameSession gameSession)
    {
        
        this.GameSessions.Add(gameSession);
    }
}
