using MePoC.Common;

namespace MePoC.Models;
public class Response : BaseModel, IAggregateModel
{
    public string Text { get; set; } = null!;
    public string AudioName { get; set; } = null!;
    public string AudioPath { get; set; } = null!;
    public DateTime Created { get; set; }

    public Response(string text, string audioName, string audioPath, DateTime created)
    {
        Text = text;
        Created = created;
        AudioPath = audioPath;
        AudioName = audioName;
    }

}
