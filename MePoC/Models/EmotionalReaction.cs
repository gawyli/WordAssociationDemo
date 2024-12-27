using MePoC.Common;

namespace MePoC.Models;
public class EmotionalReaction : BaseModel, IAggregateModel
{
    public string Name { get; set; }
    public string Description { get; set; }
    public DateTime When { get; set; }
    //public List<Reaction> Reactions { get; set; }
}
