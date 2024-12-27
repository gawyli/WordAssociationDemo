using Refit;
using System.Text.Json;
using System.Text.Json.Serialization;
using DemoChat.Audio.Models;
using DemoChat.Hume.Interfaces;
using DemoChat.Hume.Models;
using DemoChat.Repository;
using DemoChat.Utilities;


namespace DemoChat.Hume;
public class HumeAIService : IHumeService
{
    private readonly IHumeApi _humeApi;
    private readonly ILogger<HumeAIService> _logger;
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly IRepository _repository;

    public HumeAIService(ILogger<HumeAIService> logger, IHttpClientFactory httpClientFactory, IRepository repository)
    {
        _logger = logger;
        _httpClientFactory = httpClientFactory;
        _repository = repository;
        _humeApi = RestService.For<IHumeApi>(_httpClientFactory!.CreateClient("HumeApi"));
    }

    //public async Task<IList<AudioFile>> AudioToEmotions(string[] recordsIds, CancellationToken cancellationToken)
    //{
    //    var audioFiles = new List<AudioFile>();

    //    foreach (var recordId in recordsIds)
    //    {
    //        var record = _repository.GetByIdAsync<AudioFile>(recordId, cancellationToken);
    //        var zipDetails = FileUtils.ToZipFile(record!);

    //        //var jobDetails = new JobDetails();

    //        using (var stream = System.IO.File.OpenRead(zipDetails.Path))
    //        {
    //            var streamPart = new StreamPart(stream, zipDetails.Name, "application/zip");
    //            string jobId = await _humeApi.StartInferenceJob(streamPart, cancellationToken);


    //            var jobDetails = JsonSerializer.Deserialize<JobDetails>(jobId);

    //            _logger.LogInformation($"Set Job Id {jobId}");
    //            record!.SetJobId(jobId!);
    //            _logger.LogInformation("Set Is Processed to True");
    //            record!.SetIsProcessed(true);

    //            _logger.LogInformation($"Update Record {record.Id}");
    //            await _repository.UpdateAsync(record, cancellationToken);

    //            records.Add(record);
    //            await _repository.AddAsync(jobDetails!, cancellationToken: default);
    //        }
    //    }

    //    return records;
    //}

    //public async Task<string> GetJobDetails(string jobId, CancellationToken cancellationToken)
    //{
    //    var jobDetails = await _humeApi.GetJobDetails(jobId, cancellationToken);

    //    var jsonJob = JsonSerializer.Deserialize<JobDetails>(jobDetails);
    //    //await _repository.AddAsync(jobDetails, cancellationToken);

    //    return jobDetails;
    //}

    // "e6b265b5-33a9-4a46-8ea6-673879256b88"
    public async Task<IList<InferenceSourcePredictResult>> GetJobPrediction(string jobId, CancellationToken cancellationToken)
    {
        try
        {
            // {"job_id":"4e9a390e-37b0-4aee-b453-bbcb08fc32d0"}
            var response = await _humeApi.GetJobPrediction("4e9a390e-37b0-4aee-b453-bbcb08fc32d0", cancellationToken);
            var content = await response.Content.ReadAsStreamAsync();

            var jobPredictResults = JsonSerializer.Deserialize<IList<InferenceSourcePredictResult>>(content, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
                NumberHandling = JsonNumberHandling.AllowReadingFromString,
                DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
            });

            // cosmos save to unstructured storage (blob)


            return jobPredictResults!;
        }
        catch (Exception ex)
        {
            throw new Exception($"Error getting results, {ex.Message}");
        }
    }
}
