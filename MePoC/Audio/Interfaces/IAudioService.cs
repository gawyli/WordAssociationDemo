using MePoC.Models;

namespace MePoC.Audio.Interfaces;
public interface IAudioService
{
    Task<string> TranscribeAudio(Record record, CancellationToken cancellationToken);
    Task<Record> RecordAudioTest(CancellationToken cancellationToken);
    Task<Record> RecordAudio(CancellationToken cancellationToken);
    Task<Response?> GenerateAudio(string text, CancellationToken cancellationToken);
    void PlayAudio(string path);
}
