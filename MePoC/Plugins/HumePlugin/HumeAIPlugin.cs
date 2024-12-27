using MePoC.Hume.Interfaces;
using MePoC.Hume.Models;
using MePoC.Models;
using MePoC.Repository.Interfaces;
using Microsoft.SemanticKernel;
using System.ComponentModel;

namespace MePoC.Plugins.HumePlugin;
public class HumeAIPlugin
{
    private readonly ILogger<HumeAIPlugin> _logger;
    private readonly IHumeService _humeService;
    private readonly IRepository _repository;

    public HumeAIPlugin(ILogger<HumeAIPlugin> logger, IHumeService humeService, IRepository repository)
    {
        _logger = logger;
        _humeService = humeService;
        _repository = repository;
    }

    [KernelFunction]
    [Description("Run an analysis of user voice tone")]
    public async void RunVoiceAnalysis()
    {
        try
        {
            var records = await _repository.ListAsync<Record>(cancellationToken: default);
            var lastRecord = records.OrderByDescending(r => r.Created).FirstOrDefault();

            _logger.LogInformation("Running Voice Analysis");
            var record = await _humeService.AudioToEmotions([lastRecord!.Id], cancellationToken: default);
            var jobId = record.FirstOrDefault(r => r.Id == lastRecord.Id)!.JobId;

            await _repository.AddAsync(new JobDetails { JobId = jobId! }, cancellationToken: default);

            _logger.LogInformation($"Voice Analysis Processing. Job Id {jobId}");
        }
        catch (Exception ex)
        {
            throw new Exception($"Error: {ex.Message}");
        }
    }

    //[KernelFunction]
    //[Description("Get the status of the last voice analysis")]
    //public async void GetVoiceAnalysisStatus()
    //{

    //}

    [KernelFunction]
    [Description("Get list of job details")]
    public async Task<string> GetJobsDetails()
    {
        try
        {
            var jobDetails = await _repository.ListAsync<JobDetails>(cancellationToken: default);
            var lastJobDetails = jobDetails.OrderByDescending(j => j.Created).FirstOrDefault();

            return lastJobDetails!.ToString();
        }
        catch (Exception ex)
        {
            throw new Exception($"Error: {ex.Message}");
        }
    }

    [KernelFunction]
    [Description("Retrieve voice analysis job results")]
    public async Task<string> GetJobResults([Description("ID property of the JobDetails")] string jobId)
    {
        try
        {
            var inferenceJobResult = await _humeService.GetJobPrediction(jobId!, cancellationToken: default);
            //var jsonJobResult = JsonSerializer.Serialize(inferenceJobResult);

            // save to blob as json doc

            return string.Empty;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting results");
            throw new Exception($"Error getting results, {ex.Message}");
        }
    }
}