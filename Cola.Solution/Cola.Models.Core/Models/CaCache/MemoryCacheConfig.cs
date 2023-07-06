namespace Cola.Models.Core.Models.CaCache;

public class MemoryCacheConfig
{
    //当缓存项的数量超过这个值时，会使用 LRU 算法移除最近最少使用的缓存项。默认值为 null，表示没有限制
    public long? SizeLimit { get; set; }
    //设置扫描过期项的频率。默认值为 30 秒，可以根据需要调整。
    public int ExpirationScanFrequency { get; set; } = 30;
    //设置缓存项数量达到最大限制后，移除缓存项的百分比。默认值为 0.1，表示移除最近最少使用的 10% 的缓存项。
    public float CompactionPercentage { get; set; } = 0.1f;
    // 本地缓存的默认时间
    public int DefaultExpiration { get; set; } = 30;
}