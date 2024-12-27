using MePoC.Common;

namespace MePoC.Models;
public class ZipDetails : BaseModel, IAggregateModel
{
    public string Name { get; set; } = null!;
    public string Path { get; set; } = null!;

    public ZipDetails(string name, string path)
    {
        Name = name;
        Path = path;
    }
}
