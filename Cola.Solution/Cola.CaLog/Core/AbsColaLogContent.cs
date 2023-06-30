using Cola.Models.Core.Enums.Logs;
using Cola.Models.Core.Models.ColaLog;
using Microsoft.Extensions.DependencyInjection;

namespace Cola.CaLog.Core;

public class AbsColaLogContent : AbsColaLogFace
{
    protected AbsColaLogContent(EnumLogLevel logLevel, LogConfig config, IServiceCollection? service) : base(logLevel,
        config, service)
    {
    }

    public override LogResponse? WriteLog(LogInfo log)
    {
        return null;
    }
}