using MePoC.Common;

using System.Text.Json;
using System.Text.Json.Serialization;

namespace MePoC.Models;

[Serializable]
public class JobDetails : BaseModel, IAggregateModel
{
    [JsonPropertyName("job_id")]
    public string JobId { get; set; } = null!;
    public DateTime Created { get; set; } = DateTime.UtcNow;

    public JobDetails() { }

    public JobDetails(string jobId)
    {
        this.JobId = jobId;
    }


    public override string ToString()
    {
        var json = JsonSerializer.Serialize(this);

        return json;
    }
}
