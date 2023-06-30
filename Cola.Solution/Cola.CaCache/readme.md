### ColaCache 
#### 配置文件样例
```json
{
  "ColaCache": {
    // 缓存类型   NoCache = 0, 不使用缓存   InMemory = 1,   使用本地缓存    Redis = 2,      使用redis缓存   Hybrid = 3      综合模式：优先使用本地缓存，再使用redis缓存
    "CacheType": 3,
    "RedisCache": {
      // 单机模式
      "Standalone": {
        "ConnectionStrings": "ip:port,password=pwd,ssl=False,abortConnect=True"
      },
      //      // 哨兵+集群
      //      "Sentinels": {
      //        "Sentinel": "sentinel-host:port,sentinel2-host:port?sentinel=true&password=xxxxx&db=0&ssl=False&abortConnect=False&connectTimeout=5000&allowAdmin=True",
      //        "Masters": "redis-master:port,password=xxxxx,connectTimeout=5000,allowAdmin=True",
      //        "Slaves": "redis-slave:port,password=xxxxx,connectTimeout=5000,allowAdmin=True",
      //        "ServiceName": "mymaster"
      //      },
      //      //集群模式
      //      "Cluster": {
      //        "Masters": "redis-master:port,password=xxxxx,connectTimeout=5000,allowAdmin=True",
      //        "Slaves": "redis-slave:port,password=xxxxx,connectTimeout=5000,allowAdmin=True"
      //      },
      //事件总线
      "EventBus": "ip:port,password=pwd,connectTimeout=5000,allowAdmin=False,ssl=False,abortConnect=True"
    },
    "MemoryCache": {
      //当缓存项的数量超过这个值时，会使用 LRU 算法移除最近最少使用的缓存项。默认值为 null，表示没有限制
      "SizeLimit": null,
      //设置扫描过期项的频率。默认值为 30 秒，可以根据需要调整。
      "ExpirationScanFrequency": 30,
      //设置缓存项数量达到最大限制后，移除缓存项的百分比。默认值为 0.1，表示移除最近最少使用的 10% 的缓存项。
      "CompactionPercentage": 0.1
    }
  }
}
```

#### 具体调用
```csharp
// 缓存 组件
builder.Services.AddSingletonColaCache(config);

// 本地缓存
if (config.GetSection("ColaCache:CacheType").Value == "1")
{
    IColaMemoryCache memoryCache = builder.Services.BuildServiceProvider().GetService<IColaMemoryCache>()!;
    memoryCache.Set<Student>("localCache",new Student{  StdName = "local cache", StdId = 20 },new TimeSpan(UnixTimeHelper.GetUnixDateTime() + 100));
    Console.WriteLine(JsonConvert.SerializeObject(memoryCache.Get<Student>("localCache")));
}
// redis 缓存
if (config.GetSection("ColaCache:CacheType").Value == "2")
{
    IColaRedisCache redisCache = builder.Services.BuildServiceProvider().GetService<IColaRedisCache>()!;
    redisCache.Set<Student>("redisCache",new Student{  StdName = "redis cache", StdId = 20 },new TimeSpan(UnixTimeHelper.GetUnixDateTime() + 100));
    Console.WriteLine(JsonConvert.SerializeObject(redisCache.Get<Student>("redisCache")));
}
// 综合模式：优先使用本地缓存，再使用redis缓存
if (config.GetSection("ColaCache:CacheType").Value == "3")
{
    IColaHybridCache hybridCache = builder.Services.BuildServiceProvider().GetService<IColaHybridCache>()!;
    hybridCache.Set("Cache:hybrid", new Student { StdName = "hybrid cache", StdId = 20 },
        new TimeSpan(UnixTimeHelper.GetUnixDateTime() + 100),1);
    Console.WriteLine(JsonConvert.SerializeObject(hybridCache.Get<Student>("Cache:hybridCache")));
}
```
