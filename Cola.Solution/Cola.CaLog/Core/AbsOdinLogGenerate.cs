using Cola.Models.Core.Enums.Logs;
using Cola.Models.Core.Models.ColaLog;

namespace Cola.CaLog.Core;

public abstract class AbsOdinLogGenerate : IColaLog
{
    protected LogConfig? Config;
    public abstract LogModel GenerateLog(EnumLogLevel logLevel, LogInfo log);
}