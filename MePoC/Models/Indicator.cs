using MePoC.Common;

namespace MePoC.Models;
public class Indicator : BaseModel, IAggregateModel
{
    public string Name { get; set; } = null!;
    public string Description { get; set; } = null!;
    public DateTime When { get; set; }
    public WordsPair WordsPair { get; set; } = null!;

    public Indicator(string name, string description)
    {
        Name = name;
        Description = description;
        When = DateTime.UtcNow;
    }

    protected Indicator()
    {
    }
}
