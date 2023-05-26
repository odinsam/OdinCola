namespace Cola.Models.Core.Models.CaCache;

/// <summary>
///     单机模式
/// </summary>
public class StandaloneCacheConfig : RedisCacheConfig
{
    public string? ConnectionStrings { get; set; }
}