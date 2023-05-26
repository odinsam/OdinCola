namespace Cola.Models.Core.Models.CaCache;

public class RedisCacheConfig
{
    public StandaloneCacheConfig? Standalone { get; set; }

    public SentinelsCacheConfig? Sentinels { get; set; }

    public ClusterCacheConfig? Cluster { get; set; }

    public string? EventBus { get; set; }
}