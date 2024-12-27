
using MePoC.Common;

namespace MePoC.Models;
public class WordsPair : BaseModel, IAggregateModel
{
    public string SessionId { get; set; } = null!;
    public string StimulusId { get; set; } = null!;
    public Word Stimulus { get; set; } = null!;
    public string ResponseId { get; set; } = null!;
    public Word Response { get; set; } = null!;
    public DateTime ResponseTime { get; set; }

    //public List<Indicator>? ComplexIndicators { get; set; }
    //public EmotionalReaction? EmotionalReaction { get; set; }

    public virtual Session? Session { get; set; }

    public WordsPair(string sessionId, Word stimulus, Word response, DateTime responseTime)
    {
        SessionId = sessionId;
        Stimulus = stimulus;
        Response = response;
        ResponseTime = responseTime;
    }

    protected WordsPair()
    {
    }
}
