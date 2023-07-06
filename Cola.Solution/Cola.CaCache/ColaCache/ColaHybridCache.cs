using Cola.CaCache.IColaCache;
using Cola.CaException;
using Cola.CaUtils.Helper;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using StackExchange.Redis;

namespace Cola.CaCache.ColaCache;

public class ColaHybridCache : IColaHybridCache
{
    public  IColaRedisCache ColaRedisCache{ get; set; }
    public  IColaMemoryCache ColaMemoryCache{ get; set; }
    private readonly IColaException _exceptionHelper;
    
    public ColaHybridCache(IColaRedisCache colaRedis,IColaMemoryCache colaMemoryCache,IColaException exceptionHelper)
    {
        ColaRedisCache = colaRedis;
        ColaMemoryCache = colaMemoryCache;
        _exceptionHelper = exceptionHelper;
    }

    public static ColaHybridCache Create(IServiceProvider serviceProvider)
    {
        var exceptionHelper = serviceProvider.GetService<IColaException>();
        if (exceptionHelper == null) throw new Exception("未注入 IColaException 类型");
        var colaRedis = serviceProvider.GetService<IColaRedisCache>();
        if (colaRedis == null) throw new Exception("未注入 IColaRedisCache 类型");
        var colaMemoryCache = serviceProvider.GetService<IColaMemoryCache>();
        if (colaMemoryCache == null) throw new Exception("未注入 IColaMemoryCache 类型");
        return new ColaHybridCache(colaRedis, colaMemoryCache, exceptionHelper);
    }



    public T? Get<T>(string key, int database = 0)
    {
        var memoryResult = ColaMemoryCache.Get<T>(key);
        if (memoryResult == null)
        {
            var redisResult = ColaRedisCache.Get<T>(key, database);
            if (redisResult != null)
            {
                DateTime absoluteDateTime = ColaRedisCache.GetExpireTime(key, database);
                ColaMemoryCache.Set(key,redisResult,new TimeSpan(UnixTimeHelper.FromDateTime(absoluteDateTime)));
                return redisResult;
            }
            return default(T);
        }
        return memoryResult;
    }

    public bool Set<T>(string key, T value, TimeSpan? expiry = null, int database = 0)
    {
        ColaMemoryCache.Set<T>(key,value,expiry);
        ColaRedisCache.Set<T>(key,value,expiry,database);
        return true;
    }

    public void Refresh(string key, TimeSpan? expiry = null, int database = 0)
    {
        var memoryResult = ColaMemoryCache.Get<dynamic>(key);
        bool memoryExists = true;
        if (memoryResult == null)
            memoryExists = false;
        else
            ColaMemoryCache.Set(key,memoryResult,expiry.Value);
        
        var redisResult = ColaRedisCache.Get<dynamic>(key, database);
        if (redisResult == null)
        {
            if(!memoryExists)
                throw _exceptionHelper.ThrowException("key does not exist");
            ColaRedisCache.Set(key, memoryResult, expiry, database);
        }
        else
        {
            if (!memoryExists)
                ColaMemoryCache.Set(key,redisResult,expiry.Value);
            ColaRedisCache.Refresh(key,expiry,database);
        }
    }

    public bool Remove(string key, int database = 0)
    {
        ColaMemoryCache.Remove(key);
        ColaRedisCache.Remove(key, database);
        return true;
    }

    public RedisLock RedisLock(string key, TimeSpan expiry, int database = 0)
    {
        return ColaRedisCache.RedisLock(key, expiry, database);
    }

    public ReaderWriterLockSlim MemoryLock()
    {
        return ColaMemoryCache.MemoryLock();
    }

    public void RedisLockMethodAsync(string key, TimeSpan expiry, Action<ConnectionMultiplexer, ConnectionMultiplexer, ConnectionMultiplexer> success, Action<ConnectionMultiplexer, ConnectionMultiplexer, ConnectionMultiplexer, Exception> fail, int database = 0)
    {
        ColaRedisCache.RedisLockMethodAsync(key, expiry, success, fail, database);
    }

    public void MemoryLockMethodAsync<T>(Action<IMemoryCache> success, Action<IMemoryCache, Exception> fail)
    {
        ColaMemoryCache.MemoryLockMethodAsync<T>(success,fail);
    }

    public void Subscribe(string channel, Action<string, string> handler)
    {
        ColaRedisCache.Subscribe(channel, handler);
    }

    public void Publish(string channel, string message)
    {
        ColaRedisCache.Publish(channel,message);
    }

    public void Unsubscribe(string channel)
    {
        ColaRedisCache.Unsubscribe(channel);
    }

    
}