using MePoC.Common;

namespace MePoC.Models;
public class ResponseStatistic : BaseModel, IAggregateModel
{
    public Word Response { get; set; }
    public int Frequency { get; set; }
    public int HapaxLegomenaCount { get; set; }

    public ResponseStatistic(Word response, int frequency, int hapaxLegomenaCount)
    {
        Id = Guid.NewGuid().ToString();
        Response = response;
        Frequency = frequency;
        HapaxLegomenaCount = hapaxLegomenaCount;
    }
}
