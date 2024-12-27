using MePoC.Common;

namespace MePoC.Models;
public class Session : BaseModel, IAggregateModel
{
    public DateTime Created { get; set; }
    public DateTime StartTime { get; private set; }
    public List<WordsPair> WordsPairs { get; private set; } = new();
    public DateTime StopTime { get; private set; }
    public string? Validity { get; private set; }           // then this should be "low"
    public string? Reliablitiy { get; private set; }        // as this as well should be "low"
    public string? History { get; private set; }

    public Session(DateTime created)
    {
        this.Created = created;
    }

    protected Session()
    {
    }

    public void SetStartTime()
    {
        this.StartTime = DateTime.UtcNow;
    }

    public void SetStopTime()
    {
        this.StopTime = DateTime.UtcNow;
    }

    public void AddWordsPair(WordsPair wordsPair)
    {
        this.WordsPairs.Add(wordsPair);
    }

    public void AddSessionHistory(string history)
    {
        this.History += history;
    }
}
