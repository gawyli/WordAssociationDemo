﻿using Refit;

namespace DemoChat.Hume.Interfaces;
public interface IHumeApi
{
    [Multipart]
    [Post("/v0/batch/jobs")]
    Task<string> StartInferenceJob([AliasAs("file")] StreamPart audioFile, CancellationToken cancellationToken);

    [Get("/v0/batch/jobs/{jobId}")]
    Task<string> GetJobDetails(string jobId, CancellationToken cancellationToken);

    [Get("/v0/batch/jobs/{jobId}/predictions")]
    Task<HttpResponseMessage> GetJobPrediction(string jobId, CancellationToken cancellationToken);
}
