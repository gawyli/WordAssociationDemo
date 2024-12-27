using MePoC.Common;

namespace MePoC.Models;
public class Record : BaseModel, IAggregateModel
{
    public string Name { get; set; } = null!;
    public string Path { get; set; } = null!;
    public DateTime Created { get; set; }
    public bool IsProcessed { get; private set; }
    public string? JobId { get; private set; }
    // public string? SessionId { get; private set; }

    public Record(string name, string path, DateTime created, bool isProcessed = false)
    {
        Id = Guid.NewGuid().ToString();
        Name = name;
        Created = created;
        Path = path;
        IsProcessed = isProcessed;

        //TODO: Add Session ID
    }

    public void SetJobId(string jobId)
    {
        this.JobId = jobId;
    }

    public void SetIsProcessed(bool value)
    {
        this.IsProcessed = value;
    }

    //public void SetSessionId(string sessionId)
    //{
    //    this.SessionId = sessionId;
    //}
}
