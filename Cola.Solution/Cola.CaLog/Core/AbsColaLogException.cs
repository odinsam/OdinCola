using Cola.CaLog.Core;
using Cola.Models.Core.Enums.Logs;
using Cola.Models.Core.Models.ColaLog;
using Microsoft.Extensions.DependencyInjection;

namespace Cola.Log.Core;

public abstract class AbsColaLogException : AbsColaLogFace
{
    protected AbsColaLogException(EnumLogLevel logLevel, LogConfig config, IServiceCollection? service) : base(logLevel,
        config, service)
    {
    }

    public override LogResponse? WriteLog(LogInfo log)
    {
        return WriteLogContent(log);
    }
}