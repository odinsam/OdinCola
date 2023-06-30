using Cola.CaConsole;
using Cola.CaEF.BaseRepository;
using Cola.CaEF.Models;
using Cola.CaEF.Tenant;
using Cola.CaException;
using Cola.CaUtils.Constants;
using Cola.Models.Core.Models.ColaEf;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SqlSugar;

namespace Cola.CaEF.EfInject;

public static class SqlSugarInject
{
    public static IServiceCollection AddSingletonColaSqlSugar(
        this IServiceCollection services,
        IConfiguration configuration,
        IHttpContextAccessor httpContextAccessor,
        Action<ColaEfConfigOption> action,
        List<GlobalQueryFilter>? tableFilter = null,
        List<AopOnLogExecutingModel>? aopOnLogExecutingModels = null,
        List<AopOnErrorModel>? aopOnErrorModels = null)
    {
        var opts = new ColaEfConfigOption();
        action(opts);
        return InjectSqlSugar(services, opts, httpContextAccessor, configuration, tableFilter, aopOnLogExecutingModels, aopOnErrorModels);
    }

    public static IServiceCollection AddSingletonColaSqlSugar(
        this IServiceCollection services,
        IConfiguration configuration,
        IHttpContextAccessor httpContextAccessor,
        List<GlobalQueryFilter>? tableFilter = null,
        List<AopOnLogExecutingModel>? aopOnLogExecutingModels = null,
        List<AopOnErrorModel>? aopOnErrorModels = null)
    {
        var colaEfConfig = configuration.GetSection(SystemConstant.CONSTANT_COLAORM_SECTION).Get<ColaEfConfigOption>();
        var opts = new ColaEfConfigOption
        {
            TenantResolutionStrategy = colaEfConfig.TenantResolutionStrategy,
            ColaOrmConfig = colaEfConfig.ColaOrmConfig
        };
        return InjectSqlSugar(services, opts, httpContextAccessor, configuration, tableFilter, aopOnLogExecutingModels, aopOnErrorModels);
    }

    private static IServiceCollection InjectSqlSugar(
        IServiceCollection services,
        ColaEfConfigOption colaEfConfigOption,
        IHttpContextAccessor httpContextAccessor,
        IConfiguration configuration,
        List<GlobalQueryFilter>? tableFilter = null,
        List<AopOnLogExecutingModel>? aopOnLogExecutingModels = null,
        List<AopOnErrorModel>? aopOnErrorModels = null)
    {
        // 配置参数验证
        ValidateColaEfConfigOption(services, colaEfConfigOption);
        services.AddSingleton<ITenantResolutionStrategy, DomainTenantResolutionStrategy>();
        services.AddSingleton<ITenantContext>(sp => TenantContext.Create(sp,configuration,aopOnLogExecutingModels,aopOnErrorModels,tableFilter));
        
        var sqlSugarConfigLst = new List<ConnectionConfig>();
        for (var i = 0; i < colaEfConfigOption.ColaOrmConfig!.Count; i++)
        {
            var opt = colaEfConfigOption.ColaOrmConfig[i];
            sqlSugarConfigLst.Add(new ConnectionConfig
            {
                ConfigId = opt.ConfigId,
                DbType = opt.GetSqlSugarDbType(),
                ConnectionString = opt.ConnectionString,
                IsAutoCloseConnection = opt.IsAutoCloseConnection
            });
        }

        #region 参数验证

        ValidateSqlSugarConfigLst(services, sqlSugarConfigLst);

        ValidateAopOnLogExecuting(services, sqlSugarConfigLst, colaEfConfigOption, aopOnLogExecutingModels);

        ValidateAopOnError(services, sqlSugarConfigLst, colaEfConfigOption, aopOnErrorModels);

        #endregion

        InjectTenantResolutionStrategy(services, httpContextAccessor, configuration, colaEfConfigOption);
        if (sqlSugarConfigLst.Count > 0)
        {
            services.AddSingleton<ISqlSugarRepository>(SqlSugarRepository.Create);
            ConsoleHelper.WriteInfo("注入类型【 ISqlSugarRepository, SqlSugarRepository 】");
        }
        else
        {
            ConsoleHelper.WriteLine("SqlSugar配置不正确，无法类型【 ISqlSugarRepository, SqlSugarRepository 】", backgroundColor: ConsoleColor.DarkRed);
        }

        return services;
    }

    private static void InjectTenantResolutionStrategy(IServiceCollection services, IHttpContextAccessor httpContextAccessor, IConfiguration configuration, ColaEfConfigOption colaEfConfigOption)
    {
        Dictionary<string, ITenantResolutionStrategy> dicTenantResolutionStrategys =
            new Dictionary<string, ITenantResolutionStrategy>()
            {
                { "DomainTenant",new DomainTenantResolutionStrategy(httpContextAccessor, configuration, services.BuildServiceProvider()) },
                { "HttpHeaderTenant",new HttpHeaderTenantResolutionStrategy(httpContextAccessor) },
                { "RouteValueTenant",new RouteValueTenantResolutionStrategy(httpContextAccessor) },
                { "NoTenant",new NoTenantResolutionStrategy() }
            };
        var tenantResolutionStrategys = dicTenantResolutionStrategys[colaEfConfigOption.TenantResolutionStrategy];
        services.AddSingleton(tenantResolutionStrategys);
    }
    
    #region 检查参数方法

    private static void ValidateAopOnLogExecuting(IServiceCollection services, List<ConnectionConfig> sqlSugarConfigLst,
        ColaEfConfigOption opts, List<AopOnLogExecutingModel>? aopOnLogExecutingModels = null)
    {
        var exceptionHelper = services.BuildServiceProvider().GetService<IColaException>();
        if (aopOnLogExecutingModels != null)
        {
            if (aopOnLogExecutingModels.Count != sqlSugarConfigLst.Count)
                exceptionHelper!.ThrowException("AopOnLogExecuting 个数配置不正确");
            for (var i = 0; i < aopOnLogExecutingModels.Count; i++)
            {
                if (aopOnLogExecutingModels[i].AopOnLogExecuting == null)
                    exceptionHelper!.ThrowException("AopOnLogExecuting 配置不正确");
                aopOnLogExecutingModels[i].ConfigId = opts!.ColaOrmConfig![i].ConfigId ?? string.Empty;
            }
        }
    }

    private static void ValidateColaEfConfigOption(IServiceCollection services, ColaEfConfigOption option)
    {
        var exceptionHelper = services.BuildServiceProvider().GetService<IColaException>();
        if (option.ColaOrmConfig == null || option.ColaOrmConfig.Count == 0)
            exceptionHelper!.ThrowException("ColaEfConfig 配置不正确");
    }

    private static void ValidateAopOnError(IServiceCollection services, List<ConnectionConfig> sqlSugarConfigLst,
        ColaEfConfigOption opts, List<AopOnErrorModel>? aopOnErrorModels = null)
    {
        var exceptionHelper = services.BuildServiceProvider().GetService<IColaException>();
        if (aopOnErrorModels != null)
        {
            if (aopOnErrorModels.Count != sqlSugarConfigLst.Count)
                exceptionHelper!.ThrowException("AopOnLogExecuting 个数配置不正确");
            for (var i = 0; i < aopOnErrorModels.Count; i++)
            {
                if (aopOnErrorModels[i].AopOnError == null)
                    exceptionHelper!.ThrowException("AopOnError 配置不正确");
                aopOnErrorModels[i].ConfigId = opts!.ColaOrmConfig![i].ConfigId ?? string.Empty;
            }
        }
    }

    private static void ValidateSqlSugarConfigLst(IServiceCollection services, List<ConnectionConfig> sqlSugarConfigLst)
    {
        var exceptionHelper = services.BuildServiceProvider().GetService<IColaException>();
        if (sqlSugarConfigLst.Count > 1)
            if (sqlSugarConfigLst.Count(c => string.IsNullOrEmpty(c.ConfigId)) > 0)
                exceptionHelper!.ThrowException("SqlSugar 多库配置 ConfigId 必须配置");
    }

    #endregion
}