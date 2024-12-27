using MePoC.Common;
using MePoC.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MePoC.DataBus;
public class TestDetails : BaseModel
{
    public string? SessionId { get; set; }
    public Session? Session { get; set; }
    public bool IsTestInitialized { get; set; }
    public bool IsTestRunning { get; set; }
    public bool IsTestCompleted { get; set; }

    public TestDetails(bool isTestInitialized, bool isTestRunning, bool isTestCompleted)
    {
        IsTestInitialized = isTestInitialized;
        IsTestRunning = isTestRunning;
        IsTestCompleted = isTestCompleted;

    }
}
