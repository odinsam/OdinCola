using Cola.CaConsole;
using Cola.CaLog.Core;
using Cola.CaUtils.Constants;
using Cola.CaUtils.Extensions;
using Cola.Models.Core.Models.ColaLog;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Cola.CaLog;

/// <summary>
///     ColaLogInject
/// </summary>
public static class ColaLogInject
{
    public static IServiceCollection AddSingletonColaLogs(
        this IServiceCollection services,
        IConfiguration config)
    {
        var logConfig = config.GetSection(SystemConstant.CONSTANT_COLALOGS_SECTION).Get<LogConfig>();
        logConfig = logConfig ?? new LogConfig();
        var opts = new LogConfigOption { Config = logConfig };
        services.AddSingleton<IColaLogs>(provider => new ColaLogs(opts, services));
        ConsoleHelper.WriteInfo("注入类型【 IColaLogs, ColaLogs 】");
        return services;
    }

    public static IServiceCollection AddSingletonColaLogs(
        this IServiceCollection services,
        Action<LogConfigOption> action)
    {
        var opts = new LogConfigOption();
        action(opts);
        services.AddSingleton<IColaLogs>(provider => new ColaLogs(opts, services));
        ConsoleHelper.WriteInfo("注入类型【 IColaLogs, ColaLogs 】");
        return services;
    }

    public static IServiceCollection AddTransientColaLogs(
        this IServiceCollection services,
        IConfiguration config)
    {
        var logConfig = config.GetSection(SystemConstant.CONSTANT_COLALOGS_SECTION).Get<LogConfig>();
        var opts = new LogConfigOption { Config = logConfig };
        services.AddTransient<IColaLogs>(provider => new ColaLogs(opts, services));
        ConsoleHelper.WriteInfo("注入类型【 IColaLogs, ColaLogs 】");
        return services;
    }

    public static IServiceCollection AddTransientColaLogs(
        this IServiceCollection services,
        Action<LogConfigOption> action)
    {
        var opts = new LogConfigOption();
        action(opts);
        services.AddTransient<IColaLogs>(provider => new ColaLogs(opts, services));
        ConsoleHelper.WriteInfo("注入类型【 IColaLogs, ColaLogs 】");
        return services;
    }

    public static IServiceCollection AddScopedColaLogs(
        this IServiceCollection services,
        IConfiguration config)
    {
        var logConfig = config.GetSection(SystemConstant.CONSTANT_COLALOGS_SECTION).Get<LogConfig>();
        var opts = new LogConfigOption { Config = logConfig };
        services.AddScoped<IColaLogs>(provider => new ColaLogs(opts, services));
        ConsoleHelper.WriteInfo("注入类型【 IColaLogs, ColaLogs 】");
        return services;
    }

    public static IServiceCollection AddScopedColaLogs(
        this IServiceCollection services,
        Action<LogConfigOption> action)
    {
        var opts = new LogConfigOption();
        action(opts);
        services.AddScoped<IColaLogs>(provider => new ColaLogs(opts, services));
        ConsoleHelper.WriteInfo("注入类型【 IColaLogs, ColaLogs 】");
        return services;
    }
}