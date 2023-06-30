using Cola.Models.Core.Enums.Logs;
using Cola.Models.Core.Models.ColaLog;
using Microsoft.Extensions.DependencyInjection;

namespace Cola.CaLog.Core;

public abstract class AbsLogContent : AbsColaLogContent
{
    protected AbsLogContent(EnumLogLevel logLevel, LogConfig config, IServiceCollection? service) : base(logLevel,
        config, service)
    {
    }

    private LogResponse? WriteLog(Exception exception, long? logId = null)
    {
        return null;
    }
}