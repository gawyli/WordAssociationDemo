using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DemoChat.Common;
using Microsoft.SemanticKernel.ChatCompletion;

namespace DemoChat.Audio.Models;
public class AudioFile : BaseEntity
{
    public string ChatSessionId { get; set; } = null!;
    public string Name { get; set; } = null!;
    public string Path { get; set; } = null!;
    public string MimeType { get; set; } = null!;
    public AuthorRole AuthorRole { get; set; }
    public DateTime Created { get; set; }
    public string? Content { get; private set; }
    public bool? IsProcessed { get; private set; }

    public AudioFile(string chatSessionId, string name, string path, string mimeType, AuthorRole authorRole, DateTime created, bool? isProcessed = false)
    {
        this.ChatSessionId = chatSessionId;
        this.Name = name;
        this.Created = created;
        this.Path = path;
        this.MimeType = mimeType;
        this.AuthorRole = authorRole;
        this.IsProcessed = isProcessed;

    }

    // EF Core
    protected AudioFile()
    {
    }

    public void SetContent(string content)
    {
        this.Content = content;
    }

    public void SetIsProcessed(bool value)
    {
        this.IsProcessed = value;
    }
}
