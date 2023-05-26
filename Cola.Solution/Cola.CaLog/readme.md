#### ColaLog 日志组件

##### 简介

ColaLog日志组件，可以生成日志并保存。保存可以保存为文件、或保存到数据库或全部。日志文件保存路径为 bin/logs/
其中logs目录如果不存在会自动生成。

控制台 Info信息输出绿色文字、waring信息输出蓝色文字、Error信息输出红色文字

##### 配置文件配置

```json
{
  "ColaLogs":{
    "LogSeparator":"*",
    "LogTimeFormat":"yyyy-MM-dd HH:mm:ss",
    /*
    保存形式为 File、SqlServer、MySql、All、不配置， 效果分别为
    保存到 文件、sqlserver数据库、mysql数据库、所有（即保存文件又保存数据库）、仅打印
    配置方式如下
    "SaveType":"File,MySql" 或者 "SaveType":"All"  或者 "SaveType":""
    */
    "SaveType":"File",
    // 保存log的数据库链接字符串
    "DbConnectionString": "",
    // 日志文件夹名称
    "DirectoryName": "ColaLogs",
    // 日志保留时间 单位 天
    "KeepTime":10
  }
}
```

##### 注入方式

```csharp
//通过配置文件注入 AddScopedColaLogs  AddTransientColaLogs
builder.Services.AddSingletonColaLogs(config);

// 直接注入  AddScopedColaLogs  AddTransientColaLogs
builder.Services.AddSingletonColaLogs(option =>
{
    option.Config = new LogConfig()
    {
        LogTimeFormat = "yyyy-MM-dd HH:mm:ss",
        LogSeparator = "=",
        SaveType = "",
        DbConnectionString = ""
    };
});
```

##### 使用方式

````csharp
// Info 输出   控制台绿色文字
 Log.Info(exception.Message);
// Waring 输出   控制台蓝色文字
 Log.Waring(exception.StackTrace);
// Error 输出   控制台红色文字
 Log.Error(exception);
````