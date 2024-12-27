using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MePoC.DataBus;
public interface IDataBus
{
    bool IsCompleted();
    bool IsRequested();
    bool IsRunning();
    void ResetTest();
    void SetTestCompleted();
    void SetTestRequest();
    void SetTestRunning();


}
