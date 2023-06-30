#### Cola.EF 框架

##### 1. 注入

注入可以通过config配置文件形式注入（推荐）、也可以通过代码直接注入

```json 配置文件
{
  "ColaOrm": {
    "TenantResolutionStrategy": "NoTenant",
    "ColaOrmConfig": [
      {
        // 多租户模式租户id传参方式      
        // NoTenant  不使用多租户   RouteValueTenant 路由形式   HttpHeaderTenant  http头形式    DomainTenant   域名形式
        // 路由形式 和 http头形式  都必须传递  tenantId：value

        // 默认值 1
        "ConfigId": "1",
        "DbType": "MySql",
        "ConnectionString": "server=xxx.xxx.xxx.xxx;Database=db;Uid=root;Pwd=pwd;AllowLoadLocalInfile=true;",
        "IsAutoCloseConnection": true,
        "EnableLogAop": true,
        "EnableErrorAop": true,
        "EnableGlobalFilter": true
      }
    ]
  }
}
```

```csharp
// 注入 ColaSqlSugar
builder.Services.AddSingletonColaSqlSugar(config,new HttpContextAccessor(),
        tableFilter:(new List<GlobalQueryFilter>()
        {
            new GlobalQueryFilter()
            {
                ConfigId = "1",
                QueryFilter = (provider => provider.AddTableFilter<IStatus>(t => t.IsDelete == false))
            }
        }),
        aopOnLogExecutingModels:new List<AopOnLogExecutingModel>()
        {
            new AopOnLogExecutingModel()
            {
                ConfigId   = "1",
                AopOnLogExecuting = ((sql, parameters) =>
                {
                    ConsoleHelper.WriteInfo($"sql is\n{sql}");
                })
            }
        },
        aopOnErrorModels:new List<AopOnErrorModel>()
        {
            new AopOnErrorModel()
            {
                ConfigId = "1",
                AopOnError = (ConsoleHelper.WriteException)
            }
        }
    );
var dbClient = builder.Services.BuildServiceProvider().GetService<ISqlSugarRepository>();
var sqlResult = dbClient.GetAll<OdinLog>();
Console.WriteLine(sqlResult.Count);
if (sqlResult.Count > 0)
{
    Console.WriteLine(JsonConvert.SerializeObject(sqlResult[0]));
}
```

##### 2. 使用

详见 ConsoleApp1Test 
















