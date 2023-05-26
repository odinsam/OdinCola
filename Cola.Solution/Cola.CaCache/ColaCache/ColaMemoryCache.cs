using Cola.CaCache.IColaCache;
using Cola.CaUtils.Extensions;
using Cola.Models.Core.Models.CaCache;
using Microsoft.Extensions.Caching.Memory;

namespace Cola.CaCache.ColaCache;

public class ColaMemoryCache : IColaMemoryCache, IDisposable
{
    public IMemoryCache MemoryCache { get; set; }
    private readonly MemoryCacheOptions memoryCacheOptions;
    private readonly ReaderWriterLockSlim memoryLock;
    public ColaMemoryCache(CacheConfigOption options)
    {
        if (options.CacheType == CacheType.InMemory.ToInt() || options.CacheType == CacheType.Hybrid.ToInt())
        {
            memoryCacheOptions = new MemoryCacheOptions
            {
                //SizeLimit：设置缓存项的最大数量限制。当缓存项的数量超过这个值时，会使用 LRU 算法移除最近最少使用的缓存项。默认值为 null，表示没有限制
                SizeLimit = options.MemoryCache.SizeLimit,
                //设置扫描过期项的频率。默认值为 30 秒，可以根据需要调整。
                ExpirationScanFrequency = TimeSpan.FromMinutes(options.MemoryCache.ExpirationScanFrequency),
                //设置缓存项数量达到最大限制后，移除缓存项的百分比。默认值为 0.1，表示移除最近最少使用的 10% 的缓存项。
                CompactionPercentage = options.MemoryCache.CompactionPercentage
            };
            MemoryCache = new MemoryCache(memoryCacheOptions);
            memoryLock = new ReaderWriterLockSlim();
        }
    }

    public T? Get<T>(string key, int database = 0)
    {
        return MemoryCache.Get<T>(key);
    }

    public bool Set<T>(string key, T value, TimeSpan? expiry = null, int database = 0)
    {
        MemoryCache.Set<T>(key, value, expiry.Value);
        return true;
    }

    public void Refresh(string key, TimeSpan expiry)
    {
        MemoryCache.Set(key, MemoryCache.Get(key), expiry);
    }

    public void Remove(string key)
    {
        MemoryCache.Remove(key);
    }

    public ReaderWriterLockSlim MemoryLock()
    {
        return memoryLock;
    }

    public void MemoryLockMethodAsync<T>(Action<IMemoryCache> success, Action<IMemoryCache, Exception> fail)
    {
        memoryLock.EnterWriteLock();
        try
        {
            success(MemoryCache);
        }
        catch (Exception ex)
        {
            fail(MemoryCache, ex);
        }
        finally
        {
            memoryLock.ExitWriteLock();
        }
    }

    public void Dispose()
    {
        throw new NotImplementedException();
    }
}