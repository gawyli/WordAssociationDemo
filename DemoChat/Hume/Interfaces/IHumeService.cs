using DemoChat.Audio.Models;
using DemoChat.Hume.Models;

namespace DemoChat.Hume.Interfaces;
public interface IHumeService
{
    Task<string> CreateJobInference(string zipFilePath, CancellationToken cancellationToken);
    Task<string> GetJobInferenceStatus(string jobId, CancellationToken cancellationToken);
    Task<IList<InferenceSourcePredictResult>> GetJobPrediction(string jobId, CancellationToken cancellationToken);
}