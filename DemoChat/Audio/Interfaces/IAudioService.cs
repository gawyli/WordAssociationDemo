

using DemoChat.Audio.Models;

namespace DemoChat.Audio.Interfaces;
public interface IAudioService
{
    Task<string> TranscribeAudio(AudioFile audioFile, CancellationToken cancellationToken);
    Task<AudioFile> RecordAudio(string chatSessionId, CancellationToken cancellationToken);
    //Task<AudioFile> RecordAudio(string chatSessionId, CancellationToken cancellationToken);
    Task<AudioFile?> GenerateAudio(string chatSessionId, string content, CancellationToken cancellationToken);
    void PlayAudio(string path);
}
