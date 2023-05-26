using Microsoft.Extensions.Caching.Memory;

namespace Cola.CaCache.IColaCache;

public interface IColaMemoryCache : IColaCacheBase
{
    IMemoryCache MemoryCache { get; set; }
    void Refresh(string key, TimeSpan expiry);
    void Remove(string key);
    ReaderWriterLockSlim MemoryLock();
    void MemoryLockMethodAsync<T>(Action<IMemoryCache> success, Action<IMemoryCache, Exception> fail);
}