using DemoChat.Audio.Models;
using DemoChat.Hume.Models;

namespace DemoChat.Hume.Interfaces;
public interface IHumeService
{
    //Task<IList<AudioFile>> AudioToEmotions(string[] recordsIds, CancellationToken cancellationToken);
    //Task<string> GetJobDetails(string jobId, CancellationToken cancellationToken);

    Task<IList<InferenceSourcePredictResult>> GetJobPrediction(string jobId, CancellationToken cancellationToken);
}