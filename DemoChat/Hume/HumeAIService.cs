using Refit;
using System.Text.Json;
using System.Text.Json.Serialization;
using DemoChat.Audio.Models;
using DemoChat.Hume.Interfaces;
using DemoChat.Hume.Models;
using DemoChat.Hume.Models.Dtos;
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

    public async Task<string> CreateJobInference(string zipFilePath, CancellationToken cancellationToken)
    {
        using (var stream = System.IO.File.OpenRead(zipFilePath))
        {
            var zipFileName = Path.GetFileName(zipFilePath);

            var streamPart = new StreamPart(stream, zipFileName, "application/zip");
            var response = await _humeApi.StartInferenceJob(streamPart, cancellationToken);

            var jobDto = JsonSerializer.Deserialize<JobDto>(response);

            return jobDto.JobId;
        }
    }

    public async Task<string> GetJobInferenceStatus(string jobId, CancellationToken cancellationToken)
    {
        var jobDetails = await _humeApi.GetJobDetails(jobId, cancellationToken);

        var jobDto = JsonSerializer.Deserialize<JobDto>(jobDetails);
 
        return jobDto.Status;
    }

    public async Task<IList<InferenceSourcePredictResult>> GetJobPrediction(string jobId, CancellationToken cancellationToken)
    {
        try
        {
            var response = await _humeApi.GetJobPrediction(jobId, cancellationToken);
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
