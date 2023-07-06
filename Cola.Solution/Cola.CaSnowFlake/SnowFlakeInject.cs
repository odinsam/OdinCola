using System;
using Cola.CaConsole;
using Cola.CaUtils.Constants;
using Cola.CaUtils.Extensions;
using Cola.Models.Core.Models.ColaSnowFlake;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Cola.CaSnowFlake;

public static class SnowFlakeInject
{
    /// <summary>
    ///     inject SnowFlake
    /// </summary>
    /// <param name="services">IServiceCollection</param>
    /// <param name="action">action</param>
    /// <returns>IServiceCollection</returns>
    public static IServiceCollection AddSingletonSnowFlake(this IServiceCollection services,
        Action<SnowFlakeConfig> action)
    {
        var opts = new SnowFlakeConfig();
        action(opts);
        services.AddSingleton<IColaSnowFlake>(new ColaSnowFlake(opts.DatacenterId, opts.WorkerId));
        ConsoleHelper.WriteInfo("注入类型【 SnowFlake 】");
        return services;
    }

    /// <summary>
    ///     inject SnowFlake
    /// </summary>
    /// <param name="services">IServiceCollection</param>
    /// <param name="config">config</param>
    /// <returns>IServiceCollection</returns>
    public static IServiceCollection AddSingletonSnowFlake(this IServiceCollection services, IConfiguration config)
    {
        var snowFlakeConfig = config.GetSection(SystemConstant.CONSTANT_COLASNOWFLAKE_SECTION).Get<SnowFlakeConfig>();
        snowFlakeConfig = snowFlakeConfig ?? new SnowFlakeConfig();
        services.AddSingleton<IColaSnowFlake>(new ColaSnowFlake(snowFlakeConfig.DatacenterId,
            snowFlakeConfig.WorkerId));
        ConsoleHelper.WriteInfo("注入类型【 IColaSnowFlake, ColaSnowFlake 】");
        return services;
    }

    /// <summary>
    ///     inject SnowFlake
    /// </summary>
    /// <param name="services">IServiceCollection</param>
    /// <param name="datacenterId">datacenter Id</param>
    /// <param name="workId">work Id</param>
    /// <returns>IServiceCollection</returns>
    public static IServiceCollection AddSingletonSnowFlake(this IServiceCollection services, long datacenterId,
        long workId)
    {
        services.AddSingleton<IColaSnowFlake>(new ColaSnowFlake(datacenterId, workId));
        ConsoleHelper.WriteInfo("注入类型【 IColaSnowFlake, ColaSnowFlake 】");
        return services;
    }


    /// <summary>
    ///     inject SnowFlake
    /// </summary>
    /// <param name="services">IServiceCollection</param>
    /// <param name="action">action</param>
    /// <returns>IServiceCollection</returns>
    public static IServiceCollection AddTransientSnowFlake(this IServiceCollection services,
        Action<SnowFlakeConfig> action)
    {
        var opts = new SnowFlakeConfig();
        action(opts);
        services.AddTransient<IColaSnowFlake>(provider => new ColaSnowFlake(opts.DatacenterId, opts.WorkerId));
        ConsoleHelper.WriteInfo("注入类型【 IColaSnowFlake, ColaSnowFlake 】");
        return services;
    }

    /// <summary>
    ///     inject SnowFlake
    /// </summary>
    /// <param name="services">IServiceCollection</param>
    /// <param name="config">config</param>
    /// <returns>IServiceCollection</returns>
    public static IServiceCollection AddTransientSnowFlake(this IServiceCollection services, IConfiguration config)
    {
        var snowFlakeConfig = config.GetSection(SystemConstant.CONSTANT_COLASNOWFLAKE_SECTION).Get<SnowFlakeConfig>();
        services.AddTransient<IColaSnowFlake>(provider =>
            new ColaSnowFlake(snowFlakeConfig.DatacenterId, snowFlakeConfig.WorkerId));
        ConsoleHelper.WriteInfo("注入类型【 IColaSnowFlake, ColaSnowFlake 】");
        return services;
    }

    /// <summary>
    ///     inject SnowFlake
    /// </summary>
    /// <param name="services">IServiceCollection</param>
    /// <param name="datacenterId">datacenter Id</param>
    /// <param name="workId">work Id</param>
    /// <returns>IServiceCollection</returns>
    public static IServiceCollection AddTransientSnowFlake(this IServiceCollection services, long datacenterId,
        long workId)
    {
        services.AddTransient<IColaSnowFlake>(provider => new ColaSnowFlake(datacenterId, workId));
        ConsoleHelper.WriteInfo("注入类型【 IColaSnowFlake, ColaSnowFlake 】");
        return services;
    }

    /// <summary>
    ///     inject SnowFlake
    /// </summary>
    /// <param name="services">IServiceCollection</param>
    /// <param name="action">config</param>
    /// <returns>IServiceCollection</returns>
    public static IServiceCollection AddScopedSnowFlake(this IServiceCollection services,
        Action<SnowFlakeConfig> action)
    {
        var opts = new SnowFlakeConfig();
        action(opts);
        services.AddScoped<IColaSnowFlake>(provider => new ColaSnowFlake(opts.DatacenterId, opts.WorkerId));
        ConsoleHelper.WriteInfo("注入类型【 IColaSnowFlake, ColaSnowFlake 】");
        return services;
    }

    /// <summary>
    ///     inject SnowFlake
    /// </summary>
    /// <param name="services">IServiceCollection</param>
    /// <param name="config">config</param>
    /// <returns>IServiceCollection</returns>
    public static IServiceCollection AddScopedSnowFlake(this IServiceCollection services, IConfiguration config)
    {
        var snowFlakeConfig = config.GetSection(SystemConstant.CONSTANT_COLASNOWFLAKE_SECTION).Get<SnowFlakeConfig>();
        services.AddScoped<IColaSnowFlake>(provider =>
            new ColaSnowFlake(snowFlakeConfig.DatacenterId, snowFlakeConfig.WorkerId));
        ConsoleHelper.WriteInfo("注入类型【 IColaSnowFlake, ColaSnowFlake 】");
        return services;
    }

    /// <summary>
    ///     inject SnowFlake
    /// </summary>
    /// <param name="services">IServiceCollection</param>
    /// <param name="datacenterId">datacenter Id</param>
    /// <param name="workId">work Id</param>
    /// <returns>IServiceCollection</returns>
    public static IServiceCollection AddScopedSnowFlake(this IServiceCollection services, long datacenterId,
        long workId)
    {
        services.AddScoped<IColaSnowFlake>(provider => new ColaSnowFlake(datacenterId, workId));
        ConsoleHelper.WriteInfo("注入类型【 IColaSnowFlake, ColaSnowFlake 】");
        return services;
    }
}