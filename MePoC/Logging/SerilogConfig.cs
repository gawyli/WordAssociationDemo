using Serilog;
using Serilog.Debugging;
using System.Diagnostics;

namespace MePoC.Logging;
public class SerilogConfig
{
    public static void AddBootstrapLogging()
    {
        SetupSerilogSelfLogging(true);
        Log.Logger = new LoggerConfiguration()
            .WriteTo.Console()
            .CreateBootstrapLogger();
        Log.Information("Bootstrapping app");
    }

    private static void SetupSerilogSelfLogging(bool selfLogToFile)
    {
        if (selfLogToFile)
        {
            try
            {
                var file = File.CreateText("Logs/serilog-self-log.txt");
                SelfLog.Enable(TextWriter.Synchronized(file));
            }
            catch
            {
                SelfLog.Enable(msg => Debug.WriteLine(msg));
            }
        }
        else
        {
            SelfLog.Enable(msg => Debug.WriteLine(msg));
        }
    }
}
