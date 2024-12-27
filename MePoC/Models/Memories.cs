using MePoC.Common;

namespace MePoC.Models;
public class Memories : BaseModel, IAggregateModel
{
    public string ContextHistory { get; set; }
    public DateTime Created { get; set; }

    public Memories(string contextHistory)
    {
        Id = Guid.NewGuid().ToString();
        ContextHistory = contextHistory;
        Created = DateTime.UtcNow;
    }
}
