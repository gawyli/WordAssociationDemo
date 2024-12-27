using MePoC.Common;

namespace MePoC.Models;

public enum WordType
{
    Stimulus,
    Response
}

public class Word : BaseModel, IAggregateModel
{
    public string Content { get; set; } = null!;
    public WordType Type { get; set; }

    public Word(string content, WordType type)
    {
        Content = content;
        Type = type;
    }

    protected Word()
    {
    }
}
