using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MePoC.DataBus;
public class DataBus : IDataBus
{
    public bool IsTestRequested { get; set; }
    public bool IsTestRunning { get; set; }
    public bool IsTestCompleted { get; set; }

    public DataBus(bool isTestRequested = false, bool isTestRunning = false, bool isTestCompleted = false)
    {
        this.IsTestRequested = isTestRequested;
        this.IsTestRunning = isTestRunning;
        this.IsTestCompleted = isTestCompleted;
    }

    public bool IsRequested() => IsTestRequested;
    public bool IsRunning() => IsTestRunning;
    public bool IsCompleted() => IsTestCompleted;

    public void SetTestRequest()
    {
        IsTestRequested = true;
    }

    public void SetTestRunning()
    {
        IsTestRunning = true;
    }

    public void SetTestCompleted()
    {
        IsTestCompleted = true;
    }

    public void ResetTest()
    {
        IsTestRunning = false;
        IsTestRequested = false;
    }
}
