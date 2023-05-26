using Cola.Models.Core.Enums.Logs;
using Cola.Models.Core.Models.ColaLog;
using Microsoft.Extensions.DependencyInjection;

namespace Cola.CaLog.Core.LogUtils;

public class LogInfoUtil : AbsLogContent
{
    public LogInfoUtil(EnumLogLevel logLevel, LogConfig config, IServiceCollection? service) : base(logLevel, config,
        service)
    {
    }

    public override LogResponse? WriteLog(LogInfo log)
    {
        Console.ForegroundColor = ConsoleColor.Green;
        var logResult = WriteLogContent(log);
        Console.ResetColor();
        return logResult;
    }
}