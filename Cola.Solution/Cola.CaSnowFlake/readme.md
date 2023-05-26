### ColaSnowFlake 雪花ID

#### 1. 配置文件配置

```json
"ColaSnowFlake": {
  "DatacenterId": 1,
  "WorkerId": 1
}
```

#### 2. 注入方式

```csharp
//通过配置文件注入
builder.Services.AddSingletonSnowFlake(config);
// 直接注入
builder.Services.AddSingletonSnowFlake(option =>
{
    option.DatacenterId = 1;
    option.WorkerId = 1;
});
```

#### 3. 使用方式

````csharp
//生成雪花Id
var long = IColaSnowFlake.CreateSnowFlakeId();

//解析雪花ID 解析后的字符串格式: datetime_datacenterId_workerId_sequence
string str = IColaSnowFlake.AnalyzeId(long snowFlakeId);
````