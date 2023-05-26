using Cola.Models.Core.Enums.Logs;
using Cola.Models.Core.Models.ColaLog;

namespace Cola.CaLog.Core;

public interface IColaLog
{
    LogModel GenerateLog(EnumLogLevel logLevel, LogInfo log);
}