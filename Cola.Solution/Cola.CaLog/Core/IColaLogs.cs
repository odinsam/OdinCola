using Cola.Models.Core.Models.ColaLog;

namespace Cola.CaLog.Core;

public interface IColaLogs
{
    LogConfig? Config { get; set; }

    LogResponse? Info(LogInfo log);

    LogResponse? Info(string logInfo);

    LogResponse? Waring(LogInfo log);
    LogResponse? Waring(string logWaring);

    LogResponse? Error(Exception log);
    LogResponse? Error(ExceptionLog log);
    LogResponse? Error(string logError);
}