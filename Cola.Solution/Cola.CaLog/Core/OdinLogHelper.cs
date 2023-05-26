using System.Text;
using Cola.CaConsole;
using Cola.CaSnowFlake;
using Cola.CaUtils.Extensions;
using Cola.Models.Core.Enums.Logs;
using Cola.Models.Core.Models.ColaLog;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;

namespace Cola.CaLog.Core;

public class OdinLogHelper : AbsOdinLogGenerate
{
    #region GenerateLog method

    /// <summary>
    ///     生成log
    /// </summary>
    /// <param name="logLevel">log等级</param>
    /// <param name="log">log</param>
    /// <returns></returns>
    public override LogModel GenerateLog(EnumLogLevel logLevel, LogInfo log)
    {
        return GenerateMessageLogTemplate(logLevel, log);
    }

    #endregion

    #region 构造函数

    private static IServiceCollection? Services { get; set; }
    private static readonly Lazy<OdinLogHelper> Single = new(() => new OdinLogHelper());
    private readonly IColaSnowFlake _colaSnowFlake;

    private OdinLogHelper()
    {
        _colaSnowFlake = Services!.BuildServiceProvider().GetService<IColaSnowFlake>()!;
    }

    /// <summary>
    ///     单例构造函数
    /// </summary>
    /// <param name="config">config</param>
    /// <param name="service">service</param>
    /// <returns></returns>
    public static OdinLogHelper GetInstance(LogConfig? config, IServiceCollection? service)
    {
        Services = service;
        var odinLogHelper = Single.Value;
        odinLogHelper.Config = config ?? new LogConfig();
        return odinLogHelper;
    }

    #endregion

    #region private method

    private LogModel GenerateMessageLogTemplate(EnumLogLevel logLevel, LogInfo log)
    {
        if (_colaSnowFlake == null)
        {
            ConsoleHelper.WriteException("IColaSnowFlake 未注入，无法生成雪花Id ");
            throw new Exception("IColaSnowFlake 未注入，无法生成雪花Id ");
        }

        var newLogId = _colaSnowFlake!.CreateSnowFlakeId();
        var logid = log.LogId != null ? log.LogId.ToString() : newLogId.ToString();
        var builder = new StringBuilder();
        var separator = GenerateLogSeparator();
        builder.Append($"【 LogId 】: {logid} \r\n");
        builder.Append($"【 Log Level 】: {logLevel.ToString()} \r\n");
        builder.Append($"【 LogTime 】: {DateTime.Now.ToString(Config?.LogTimeFormat)} \r\n");
        if (log is ExceptionLog)
        {
            builder.Append($"【 Exception Message 】: {(log as ExceptionLog)?.LogException?.Message}\r\n");
            var ex = (log as ExceptionLog)?.LogException;
            builder.Append($"【 Exception Info 】: \r\n{JsonConvert.SerializeObject(ex).ToJsonFormatString()}\r\n");
        }
        else
        {
            builder.Append($"【 LogContent 】:\r\n{log.LogContent}\r\n");
        }

        builder.Append(separator + "\r\n");
        builder.Append("\r\n");
        return new LogModel { LogId = logid, LogContent = builder.ToString() };
    }

    private string GenerateLogSeparator()
    {
        var logConfig = Config;
        if (logConfig != null)
        {
            var c = logConfig.LogSeparator![0];
            var separator = logConfig.LogSeparator.PadLeft(100, c);
            return separator;
        }

        return "=";
    }

    #endregion
}