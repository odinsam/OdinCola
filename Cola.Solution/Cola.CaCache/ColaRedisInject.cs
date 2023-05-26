using Cola.CaCache.ColaCache;
using Cola.CaCache.IColaCache;
using Cola.CaConsole;
using Cola.CaUtils.Constants;
using Cola.CaUtils.Extensions;
using Cola.Models.Core.Models.CaCache;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Cola.CaCache;

public static class ColaRedisInject
{
    public static IServiceCollection AddSingletonColaCache(
        this IServiceCollection services,
        IConfiguration config)
    {
        var cacheConfig = config.GetSection(SystemConstant.CONSTANT_COLACACHE_SECTION).Get<CacheConfigOption>();
        return InjectCache(services, cacheConfig);
    }
    
    
    public static IServiceCollection AddSingletonColaCache(
        this IServiceCollection services,
        Action<CacheConfigOption> action)
    {
        var cacheConfig = new CacheConfigOption();
        action(cacheConfig);
        return InjectCache(services, cacheConfig);
    }

    private static IServiceCollection InjectCache(IServiceCollection services, CacheConfigOption cacheConfig)
    {
        if (cacheConfig.CacheType == CacheType.NoCache.ToInt())
        {
            return services;
        }
        if (cacheConfig.CacheType == CacheType.Hybrid.ToInt())
        {
            services.AddSingleton<IColaRedisCache>(provider => new ColaRedis(cacheConfig));
            ConsoleHelper.WriteInfo("注入类型【 ColaRedis, IColaRedisCache 】");
            services.AddSingleton<IColaMemoryCache>(provider => new ColaMemoryCache(cacheConfig));
            ConsoleHelper.WriteInfo("注入类型【 ColaMemoryCache, IColaMemoryCache 】");
            services.AddSingleton<IColaHybridCache,ColaHybridCache>();
            services.AddSingleton<IColaHybridCache>(s=>ColaHybridCache.Create(s));
            ConsoleHelper.WriteInfo("注入类型【 ColaHybridCache, IColaHybridCache 】");
        }
        else if (cacheConfig.CacheType == CacheType.Redis.ToInt())
        {
            services.AddSingleton<IColaRedisCache>(provider => new ColaRedis(cacheConfig));
            ConsoleHelper.WriteInfo("注入类型【 ColaRedis, IColaRedisCache 】");
        }
        else if (cacheConfig.CacheType == CacheType.InMemory.ToInt())
        {
            services.AddSingleton<IColaMemoryCache>(provider => new ColaMemoryCache(cacheConfig));
            ConsoleHelper.WriteInfo("注入类型【 ColaMemoryCache, IColaMemoryCache 】");
        }
        return services;
    }
}