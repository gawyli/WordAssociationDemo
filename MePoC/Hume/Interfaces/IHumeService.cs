using MePoC.Hume.Models;
using MePoC.Models;

namespace MePoC.Hume.Interfaces;
public interface IHumeService
{
    Task<IList<Record>> AudioToEmotions(string[] recordsIds, CancellationToken cancellationToken);
    //Task<string> GetJobDetails(string jobId, CancellationToken cancellationToken);

    Task<IList<InferenceSourcePredictResult>> GetJobPrediction(string jobId, CancellationToken cancellationToken);
}