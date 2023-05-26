using Cola.Models.Core.Enums.Logs;
using Cola.Models.Core.Models.ColaLog;
using Microsoft.Extensions.DependencyInjection;

namespace Cola.CaLog.Core;

public class ColaLogs : IColaLogs
{
    public ColaLogs(LogConfigOption opt, IServiceCollection serviceProvider)
    {
        Config = opt.Config;
        Service = serviceProvider;
    }

    public IServiceCollection Service { get; set; }
    public LogConfig? Config { get; set; }

    public LogResponse? Info(LogInfo log)
    {
        var response = ColaLogFactory.GetOdinLogUtils(EnumLogLevel.Info, Config, Service)?.WriteLog(log);
        // Console.WriteLine(JsonConvert.SerializeObject(log));
        return response;
    }

    public LogResponse? Info(string logInfo)
    {
        var response = ColaLogFactory.GetOdinLogUtils(EnumLogLevel.Info, Config, Service)
            ?.WriteLog(new LogInfo { LogContent = logInfo });
        return response;
    }

    public LogResponse? Waring(LogInfo log)
    {
        var response = ColaLogFactory.GetOdinLogUtils(EnumLogLevel.Waring, Config, Service)?.WriteLog(log);
        // Console.WriteLine(JsonConvert.SerializeObject(log));
        return response;
    }

    public LogResponse? Waring(string logWaring)
    {
        var response = ColaLogFactory.GetOdinLogUtils(EnumLogLevel.Waring, Config, Service)
            ?.WriteLog(new LogInfo { LogContent = logWaring });
        return response;
    }

    public LogResponse? Error(ExceptionLog log)
    {
        var response = ColaLogFactory.GetOdinLogUtils(EnumLogLevel.Error, Config, Service)?.WriteLog(log);
        // Console.WriteLine(JsonConvert.SerializeObject(log));
        return response;
    }

    public LogResponse? Error(Exception ex)
    {
        var response = ColaLogFactory.GetOdinLogUtils(EnumLogLevel.Error, Config, Service)
            ?.WriteLog(new ExceptionLog { LogException = ex });
        // Console.WriteLine(JsonConvert.SerializeObject(log));
        return response;
    }

    public LogResponse? Error(string logError)
    {
        var response = ColaLogFactory.GetOdinLogUtils(EnumLogLevel.Error, Config, Service)
            ?.WriteLog(new ExceptionLog { LogException = new Exception(logError) });
        return response;
    }
}