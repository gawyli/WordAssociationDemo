using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DemoChat.Common;

namespace DemoChat.Emotions.Models;
public class EmotionsSession : BaseEntity
{
    public string GameSessionId { get; set; } = null!;
    public string? JobId { get; set; }
    public string? ZipFilePath { get; set; }

    public EmotionsSession(string gameSessionId)
    {
        this.GameSessionId = gameSessionId;
    }
}
