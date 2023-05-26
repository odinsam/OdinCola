using Cola.Models.Core.Enums.Logs;
using Cola.Models.Core.Models.ColaLog;
using Microsoft.Extensions.DependencyInjection;

namespace Cola.CaLog.Core.LogUtils;

public class LogWaringUtil : AbsLogContent
{
    public LogWaringUtil(EnumLogLevel logLevel, LogConfig config, IServiceCollection? service) : base(logLevel, config,
        service)
    {
    }

    public override LogResponse? WriteLog(LogInfo log)
    {
        Console.ForegroundColor = ConsoleColor.DarkBlue;
        var logResult = WriteLogContent(log);
        Console.ResetColor();
        return logResult;
    }
}