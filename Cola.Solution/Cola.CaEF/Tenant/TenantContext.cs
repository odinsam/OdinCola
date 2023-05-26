using System.Collections.Concurrent;
using Cola.CaCache.IColaCache;
using Cola.CaConsole;
using Cola.CaEF.EntityBase;
using Cola.CaEF.Models;
using Cola.CaException;
using Cola.CaUtils.Constants;
using Cola.CaUtils.Extensions;
using Cola.CaUtils.Helper;
using Cola.Models.Core.Models.CaCache;
using Cola.Models.Core.Models.ColaEf;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using SqlSugar;

namespace Cola.CaEF.Tenant;

public class TenantContext : ITenantContext
{
    private readonly IConfiguration _configuration;
    private readonly ITenantResolutionStrategy _tenantResolutionStrategy;
    private readonly IColaCacheBase? _colaCache;
    private static readonly ConcurrentDictionary<int, SqlSugarClient> _dbClients = new ConcurrentDictionary<int, SqlSugarClient>();
    private readonly IColaException _colaException;
    private readonly ColaEfConfigOption efConfig;
    public TenantContext(ITenantResolutionStrategy tenantResolutionStrategy,
        IConfiguration configuration,
        IColaCacheBase? colaCache,
        IColaException colaException,
        List<AopOnLogExecutingModel>? aopOnLogExecutingModels,
        List<AopOnErrorModel>? aopOnErrorModels,
        List<GlobalQueryFilter>? globalQueryFilters)
    {
        _tenantResolutionStrategy = tenantResolutionStrategy;
        _configuration = configuration;
        efConfig = _configuration.GetSection(SystemConstant.CONSTANT_COLAORM_SECTION).Get<ColaEfConfigOption>();
        _colaCache = colaCache;
        _colaException = colaException;
        SetDbClientByTenant(aopOnLogExecutingModels, aopOnErrorModels, globalQueryFilters);
    }

    public static TenantContext Create(IServiceProvider serviceProvider, IConfiguration configuration,List<AopOnLogExecutingModel>? aopOnLogExecutingModels, List<AopOnErrorModel>? aopOnErrorModels, List<GlobalQueryFilter>? globalQueryFilters)
    {
        var exceptionHelper = serviceProvider.GetService<IColaException>();
        if (exceptionHelper == null) throw new Exception("未注入 IColaException 类型");
        if (configuration == null) throw new Exception("未注入 IConfiguration 类型");
        
        var tenantResolutionStrategy = serviceProvider.GetService<ITenantResolutionStrategy>();
        if (tenantResolutionStrategy == null) throw new Exception("未注入 ITenantResolutionStrategy 类型");
        
        var cacheConfig = configuration.GetSection(SystemConstant.CONSTANT_COLACACHE_SECTION).Get<CacheConfigOption>();
        IColaCacheBase colaCache = null;
        if (cacheConfig.CacheType == CacheType.Hybrid.ToInt())
        {
            var colaHybridCache = serviceProvider.GetService<IColaHybridCache>();
            if (colaHybridCache == null) throw new Exception("未注入 IColaHybridCache 类型");
            colaCache = colaHybridCache;
        }
        else if (cacheConfig.CacheType == CacheType.Redis.ToInt())
        {
            var colaRedisCache = serviceProvider.GetService<IColaRedisCache>();
            if (colaRedisCache == null) throw new Exception("未注入 IColaRedisCache 类型");
            colaCache  = colaRedisCache;
        }
        else if (cacheConfig.CacheType == CacheType.InMemory.ToInt())
        {
            var colaMemoryCache = serviceProvider.GetService<IColaMemoryCache>();
            if (colaMemoryCache == null) throw new Exception("未注入 IColaMemoryCache 类型");
            colaCache  = colaMemoryCache;
        }
        return new TenantContext(tenantResolutionStrategy, configuration, colaCache, exceptionHelper, aopOnLogExecutingModels, aopOnErrorModels, globalQueryFilters);
    }

    public int? TenantId
    {
        get
        {
            
            var tenantKey = _tenantResolutionStrategy.GetTenantResolutionKey();
            if (_colaCache != null)
            {
                string? tenantId = _colaCache.Get<string>(tenantKey!);
                if (tenantId == null)
                {
                    tenantId = _tenantResolutionStrategy.ResolveTenantKey();
                    _colaCache.Set(tenantKey!, tenantId, new TimeSpan(UnixTimeHelper.FromDateTime(DateTime.Now.AddYears(1))));
                }
                return tenantId.ToInt();
            }
            return _tenantResolutionStrategy.ResolveTenantKey().ToInt();
        }
    }

    public void SetDbClientByTenant(List<AopOnLogExecutingModel>? aopOnLogExecutingModels,List<AopOnErrorModel>? aopOnErrorModels, List<GlobalQueryFilter>? globalQueryFilters)
    {
        foreach (var config in efConfig.ColaOrmConfig!)
        {
            var connectionConfig = new ConnectionConfig
            {
                ConfigId = config.ConfigId,
                // 配置连接字符串
                ConnectionString = config.ConnectionString,
                DbType = config.DbType!.ConvertStringToEnum<DbType>(),
                IsAutoCloseConnection = config.IsAutoCloseConnection,
                InitKeyType = InitKeyType.Attribute
            };
            var client = new SqlSugarClient(connectionConfig);

            #region OnLogExecuting

            if (config.EnableLogAop)
            {
                if (aopOnLogExecutingModels != null)
                {
                    var aopOnLogExecutingModel = aopOnLogExecutingModels.SingleOrDefault(m => m.ConfigId == config.ConfigId);
                    if (aopOnLogExecutingModel != null)
                        client.Aop.OnLogExecuting = aopOnLogExecutingModel.AopOnLogExecuting;
                }
                else
                {
                    client.Aop.OnLogExecuting = (sql, parameters) =>
                    {
                        ConsoleHelper.WriteInfo($"sql:\n\t{sql}");
                        ConsoleHelper.WriteInfo("parameters is :");
                        foreach (var parameter in parameters)
                        {
                            Console.WriteLine($"\tparameter name:{parameter.ParameterName}\tparameter value:{parameter.Value.ToString()}");
                        }
                    };
                }
            }

            #endregion
            
            #region OnError

            if (config.EnableErrorAop)
            {
                if (aopOnErrorModels != null)
                {
                    var aopOnErrorModel = aopOnErrorModels.SingleOrDefault(m => m.ConfigId == config.ConfigId);
                    if (aopOnErrorModel != null)
                        client.Aop.OnError = aopOnErrorModel.AopOnError;
                }
                else
                {
                    client.Aop.OnError = exception =>
                    {
                        ConsoleHelper.WriteException($"Sql Error:");
                        ConsoleHelper.WriteException(JsonConvert.SerializeObject(exception).ToJsonFormatString());
                    };
                }
            }

            #endregion

            #region GlobalFilter

            if (config.EnableGlobalFilter)
            {
                if (globalQueryFilters != null)
                {
                    var globalQueryFilter = globalQueryFilters.SingleOrDefault(m => m.ConfigId == config.ConfigId);
                    if (globalQueryFilter != null)
                        globalQueryFilter.QueryFilter!(client.QueryFilter);
                }
                else
                {
                    client.QueryFilter.AddTableFilter<IDeleted>(t => t.IsDelete == false);
                }
            }
            
            #endregion
            
            _dbClients.TryAdd(config.ConfigId!.ToInt(), client);
        }
    }
    
    public SqlSugarClient GetDbClientByTenant(int? tenantId)
    {
        if(tenantId==null) throw _colaException.ThrowException($"{tenantId} 无法找到对应的tenantKey");
        if (_dbClients.TryGetValue(tenantId.Value, out SqlSugarClient? sqlSugarClient))
        {
            return sqlSugarClient;
        }
        throw _colaException.ThrowException($"{tenantId} 无法找到对应的链接对象");
    }
}