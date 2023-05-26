using Cola.Log.Core;
using Cola.Models.Core.Enums.Logs;
using Cola.Models.Core.Models.ColaLog;
using Microsoft.Extensions.DependencyInjection;

namespace Cola.CaLog.Core;

public abstract class AbsLogException : AbsColaLogException
{
    protected AbsLogException(EnumLogLevel logLevel, LogConfig config, IServiceCollection? service) : base(logLevel,
        config, service)
    {
    }

    private LogResponse? WriteLog(string logContent)
    {
        return null;
    }
}