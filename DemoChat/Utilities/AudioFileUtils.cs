using System;
using System.Collections.Generic;
using System.IO.Enumeration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.SemanticKernel.ChatCompletion;

namespace DemoChat.Utilities;
public static class AudioFileUtils
{
    public static (string, string) GenerateFileDetails(AuthorRole authorRole, string chatSessionId, string mimeType)
    {
        string baseDirectory = AppDomain.CurrentDomain.BaseDirectory;
        string sessionFolderName = "Sessions";
        string sessionName = $"session-{chatSessionId}";

        string subfolderName = string.Empty;
        string fileName = string.Empty;
        string audioPath = string.Empty;
        string outputPath = string.Empty;

        if (authorRole == AuthorRole.Assistant)
        {
            subfolderName = "AssistantAudio";
            audioPath = Path.Combine(baseDirectory, sessionFolderName, sessionName, subfolderName);

            fileName = $"audio-assistant-{Guid.NewGuid()}.{mimeType}";
            outputPath = Path.Combine(audioPath, fileName);
        }

        if (authorRole == AuthorRole.User)
        {
            subfolderName = "UserAudio";
            audioPath = Path.Combine(baseDirectory, sessionFolderName, sessionName, subfolderName);

            fileName = $"audio-user-{Guid.NewGuid()}.{mimeType}";
            outputPath = Path.Combine(audioPath, fileName);
        }

        if (!Directory.Exists(audioPath))
        {
            //_logger.LogInformation($"Creating directory {fullPath}");
            Directory.CreateDirectory(audioPath);
        }

        return (outputPath, fileName);
    }
}
