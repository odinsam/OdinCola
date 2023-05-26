using System.Reflection;
using Cola.Models.Core.Enums.Logs;
using Cola.Models.Core.Models.ColaLog;
using Microsoft.Extensions.DependencyInjection;

namespace Cola.CaLog.Core;

/// <summary>
///     ColaLogFactory
/// </summary>
public class ColaLogFactory
{
    public static IColaLogFace? GetOdinLogUtils(EnumLogLevel logLevel, LogConfig? config, IServiceCollection service)
    {
        var ns = "Cola.CaLog.Core.LogUtils";
        var classFullName = $"{ns}.Log{logLevel.ToString()}Util";
        var clsName = Assembly.GetExecutingAssembly().GetType(classFullName);
        if (clsName != null)
            return Activator.CreateInstance(clsName, logLevel, config, service) as IColaLogFace;
        return null;
    }
}