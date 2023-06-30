using Cola.Models.Core.Enums.Logs;
using Cola.Models.Core.Models.ColaLog;

namespace Cola.CaLog.Core;

public interface IColaLogFace
{
    EnumLogLevel LogLevel { get; set; }
    LogConfig? Config { get; set; }

    /// <summary>
    ///     写入日志
    /// </summary>
    /// <param name="log">log</param>
    LogResponse? WriteLog(LogInfo log);
}