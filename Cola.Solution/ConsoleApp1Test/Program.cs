// See https://aka.ms/new-console-template for more information

using System.Net.Sockets;
using System.Reflection;
using Cola.CaCache;
using Cola.CaCache.IColaCache;
using Cola.CaConsole;
using Cola.CaEF.BaseRepository;
using Cola.CaEF.EfInject;
using Cola.CaEF.Models;
using Cola.CaException;
using Cola.CaGrpc;
using Cola.CaGrpc.ColaGrpcClientInterceptor;
using Cola.CaGrpc.IPC;
using Cola.CaLog;
using Cola.CaLog.Core;
using Cola.CaSnowFlake;
using Cola.CaUtils.Extensions;
using Cola.CaUtils.Helper;
using Cola.CaWebApi;
using Cola.Models.Core.Models.ColaLog;
using ConsoleApp1Test;
using ConsoleApp1Test.EFTest;
using Google.Protobuf.Collections;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using Grpc.Core.Interceptors;
using Grpc.Net.Client;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using WebApi.Protos;

var builder = WebApplication.CreateBuilder(args);
builder.Configuration.AddJsonFile("appsettings.json");
var config = builder.Configuration;

// ColaException 注入
builder.Services.AddColaExceptionSingleton();
Console.WriteLine();Console.WriteLine();
//雪花Id注入
builder.Services.AddSingletonSnowFlake(config);
Console.WriteLine();Console.WriteLine();
//日志组件注入
builder.Services.AddSingletonColaLogs(config);
var Log = builder.Services.BuildServiceProvider().GetService<IColaLogs>();
Log.Info(new LogInfo
{
    LogContent = "canguan"
});
Log.Waring("test log");
Log.Error(new ExceptionLog
{
    LogException = new Exception("log error")
});
Log.Error("this is error");
Console.WriteLine();Console.WriteLine();

// 缓存 组件
builder.Services.AddSingletonColaCache(config);

// 本地缓存
if (config.GetSection("ColaCache:CacheType").Value == "1")
{
    var memoryCache = builder.Services.BuildServiceProvider().GetService<IColaMemoryCache>()!;
    memoryCache.Set("localCache", new Student { StdName = "local cache", StdId = 20 },
        new TimeSpan(UnixTimeHelper.GetUnixDateTime() + 100));
    Console.WriteLine(JsonConvert.SerializeObject(memoryCache.Get<Student>("localCache")));
}

// redis 缓存
if (config.GetSection("ColaCache:CacheType").Value == "2")
{
    var redisCache = builder.Services.BuildServiceProvider().GetService<IColaRedisCache>()!;
    redisCache.Set("redisCache", new Student { StdName = "redis cache", StdId = 20 },
        new TimeSpan(UnixTimeHelper.GetUnixDateTime() + 100));
    Console.WriteLine(JsonConvert.SerializeObject(redisCache.Get<Student>("redisCache")));
}

// 综合模式：优先使用本地缓存，再使用redis缓存
if (config.GetSection("ColaCache:CacheType").Value == "3")
{
    var hybridCache = builder.Services.BuildServiceProvider().GetService<IColaHybridCache>()!;
    hybridCache.Set("Cache:hybrid", new Student { StdName = "hybrid cache", StdId = 20 },
        new TimeSpan(UnixTimeHelper.GetUnixDateTime() + 100), 1);
    Console.WriteLine(JsonConvert.SerializeObject(hybridCache.Get<Student>("Cache:hybridCache")));
}
Console.WriteLine();Console.WriteLine();

// 注入 ColaSqlSugar
builder.Services.AddSingletonColaSqlSugar(config, new HttpContextAccessor(),
    new List<GlobalQueryFilter>
    {
        new()
        {
            ConfigId = "1",
            QueryFilter = provider => provider.AddTableFilter<IStatus>(t => t.IsDelete == false)
        }
    },
    new List<AopOnLogExecutingModel>
    {
        new()
        {
            ConfigId = "1",
            AopOnLogExecuting = (sql, parameters) => { ConsoleHelper.WriteInfo($"sql is\n{sql}"); }
        }
    },
    new List<AopOnErrorModel>
    {
        new()
        {
            ConfigId = "1",
            AopOnError = ConsoleHelper.WriteException
        }
    }
);
var dbClient = builder.Services.BuildServiceProvider().GetService<ISqlSugarRepository>();
var sqlResult = dbClient.GetAll<OdinLog>();
Console.WriteLine(sqlResult.Count);
if (sqlResult.Count > 0) Console.WriteLine(JsonConvert.SerializeObject(sqlResult[0]));
Console.WriteLine();Console.WriteLine();
// webApi 组件  
builder.Services.AddSingletonColaWebApi(config);
var webApi = builder.Services.BuildServiceProvider().GetService<IWebApi>();
var colaClient = webApi!.GetClient("Cola");
var getWebApiResult = colaClient.GetWebApiAsync<Result>(
    "https://tenapi.cn/v2/getip",
    new Dictionary<string, string>
    {
        { "author", "odinsam" }
    }).GetAwaiter().GetResult();
Console.WriteLine(JsonConvert.SerializeObject(getWebApiResult).ToJsonFormatString());
Console.WriteLine();Console.WriteLine();
//酒桶令牌算法测试
// var _tokenBucket = new TokenBucket(10, 1);
// for (var k = 0; k < 5; k++)
// {
//     if (!_tokenBucket.TryAcquire())
//         Console.WriteLine($"{k}  {_tokenBucket.Tokens}  当前没有可访问的令牌");
//     else
//         Console.WriteLine($"{k}  {_tokenBucket.Tokens}  true");
//     Thread.Sleep(500);
// }


// 注入grpc客户端
var _token = "asdf";
builder.Services
    .AddGrpcClient<Greeter.GreeterClient>(o =>
    {
        o.Address = new Uri("https://localhost:5005");
        o.Interceptors.Add(new ClientGrpcInterceptor(builder.Services.BuildServiceProvider().GetService<IColaLogs>()!,
            config));
    })
    .AddCallCredentials((context, metadata) =>
    {
        if (!string.IsNullOrEmpty(_token))
        {
            metadata.Add("Authorization", $"odinsam Bearer {_token}");
        }
        return Task.CompletedTask;
    })
    .ConfigureChannel(options =>
    {
        options.CreateGrpcClientChannelOptions(config);
    });
var client = builder.Services.BuildServiceProvider().GetService<Greeter.GreeterClient>();


// IPC进程内调用grpc
//net core 3.x 显式的指定HTTP/2不需要TLS支持
// AppContext.SetSwitch("System.Net.Http.SocketsHttpHandler.Http2UnencryptedSupport", true);

// var credentials = CallCredentials.FromInterceptor((context, metadata) =>
// {
//     if (!string.IsNullOrEmpty(_token))
//     {
//         metadata.Add("Authorization", $"Bearer {_token}");
//     }
//     return Task.CompletedTask;
// });
// var channel = new ColaGrpcHelper(new ColaWindowsGrpc()).CreateChannel("https://localhost:5005",config,null,credentials);
// var invoker =
//     channel.Intercept(new ClientGrpcInterceptor(builder.Services.BuildServiceProvider().GetService<IColaLogs>()!,
//         config));
// var client = new Greeter.GreeterClient(invoker);

// 一元调用
ConsoleHelper.WriteInfo("一元调用 start");
var headers = new Metadata();
headers.Add("customHeader","odin-custom-header");
var helloReply = client.SayHelloAsync(new HelloRequest
{
    Name = "odin-sam"
},headers);
var resultOnce = await helloReply;
Console.WriteLine(resultOnce.Message);
ConsoleHelper.WriteInfo("一元调用 over");
Console.WriteLine();Console.WriteLine();

// 客户端流调用
ConsoleHelper.WriteInfo("客户端流调用 start");
var clientStreamMethodAsync = client.ClientStreamMethodAsync();
var requestStream = clientStreamMethodAsync.RequestStream;
for (int k = 1; k < 11; k++)
{
    await requestStream.WriteAsync(new ClientStreamMethodParam { Par = k });
    Console.WriteLine($"send par {k}");
    await Task.Delay(500);
}
await requestStream.CompleteAsync();
var resultClientStream = await clientStreamMethodAsync;
Console.WriteLine($"server response result： {resultClientStream.Result}");
ConsoleHelper.WriteInfo("客户端流调用 over");
Console.WriteLine();Console.WriteLine();

//服务端流调用
ConsoleHelper.WriteInfo("服务端流调用 start");
var param = new ClientMethodParam();
param.Lst.Add(new RepeatedField<int>() { 1, 2, 3, 4, 5 });
var clientServerStreamMethodAsync = client.ServerStreamMethodAsync(param);
var responseStream = clientServerStreamMethodAsync.ResponseStream;
// Define the cancellation token.
CancellationTokenSource source = new CancellationTokenSource();
CancellationToken token = source.Token;
while (await responseStream.MoveNext(token))
{
    Console.WriteLine($"server response result { responseStream.Current.Result }");
}
ConsoleHelper.WriteInfo("服务端流调用 over");
Console.WriteLine();Console.WriteLine();

//双向流调用
ConsoleHelper.WriteInfo("双向流调用 start");
var clientServerStream = client.ClientServerStreamMethodAsync();
var clientServerRequestStream = clientServerStream.RequestStream;
for (int k = 0; k < 10; k++)
{
    await clientServerRequestStream.WriteAsync(new ClientStreamMethodParam { Par = k });
    Console.WriteLine($"send par {k}");
    await Task.Delay(500);
}
await clientServerRequestStream.CompleteAsync();
var clientServerResponse = clientServerStream.ResponseStream;
while (await clientServerResponse.MoveNext(token))
{
    Console.WriteLine($"{ clientServerResponse.Current.Result }");
}
ConsoleHelper.WriteInfo("双向流调用 over");
Console.WriteLine();Console.WriteLine();

ConsoleHelper.WriteInfo("一元复杂调用 start");
var par = new ServiceMethodParamts
{
    Id = 100,
    Name = "ali yun",
    IsExists = true,
};
par.Roles.Add(new []{"刘备","关羽","张飞"});
par.Attributes.Add(new Dictionary<string, string>()
{
    {"董卓","吕布"},
    {"王允","貂蝉"}
});
par.Detail = Any.Pack(new Person{ Start = Timestamp.FromDateTimeOffset(DateTime.Now)});
var testResult = await client.ServiceMethodAsync(par);
if (testResult.ResultCase == ServiceMethodResponse.ResultOneofCase.Person)
{
    Console.WriteLine(JsonConvert.SerializeObject(testResult.Person).ToJsonFormatString());
}
ConsoleHelper.WriteInfo("一元复杂调用 over");
Console.WriteLine();Console.WriteLine();
// // AppContext.SetSwitch("System.Net.Http.SocketsHttpHandler.Http2UnencryptedSupport", true);
// var channel = GrpcChannel.ForAddress("http://localhost:5004");

// var channel = new ColaGrpcHelper(new ColaWindowsGrpc()).CreateChannel("https://localhost:5005");
// var client = new Greeter.GreeterClient(channel);
//
// // 一元调用
// ConsoleHelper.WriteInfo("一元调用 start");
// var helloReply = client.SayHelloAsync(new HelloRequest
// {
//     Name = "odin-sam"
// });
// var resultOnce = await helloReply;
// Console.WriteLine(resultOnce.Message);
// ConsoleHelper.WriteInfo("一元调用 over");
// Console.WriteLine();Console.WriteLine();
//
// // 客户端流调用
// ConsoleHelper.WriteInfo("客户端流调用 start");
// var clientStreamMethodAsync = client.ClientStreamMethodAsync();
// var requestStream = clientStreamMethodAsync.RequestStream;
// for (int k = 1; k < 11; k++)
// {
//     await requestStream.WriteAsync(new ClientStreamMethodParam { Par = k });
//     Console.WriteLine($"send par {k}");
//     await Task.Delay(500);
// }
// await requestStream.CompleteAsync();
// var resultClientStream = await clientStreamMethodAsync;
// Console.WriteLine($"server response result： {resultClientStream.Result}");
// ConsoleHelper.WriteInfo("客户端流调用 over");
// Console.WriteLine();Console.WriteLine();
//
// //服务端流调用
// ConsoleHelper.WriteInfo("服务端流调用 start");
// var param = new ClientMethodParam();
// param.Lst.Add(new RepeatedField<int>() { 1, 2, 3, 4, 5 });
// var clientServerStreamMethodAsync = client.ServerStreamMethodAsync(param);
// var responseStream = clientServerStreamMethodAsync.ResponseStream;
// // Define the cancellation token.
// CancellationTokenSource source = new CancellationTokenSource();
// CancellationToken token = source.Token;
// while (await responseStream.MoveNext(token))
// {
//     Console.WriteLine($"server response result { responseStream.Current.Result }");
// }
// ConsoleHelper.WriteInfo("服务端流调用 over");
// Console.WriteLine();Console.WriteLine();
//
// //双向流调用
// ConsoleHelper.WriteInfo("双向流调用 start");
// var clientServerStream = client.ClientServerStreamMethodAsync();
// var clientServerRequestStream = clientServerStream.RequestStream;
// for (int k = 0; k < 10; k++)
// {
//     await clientServerRequestStream.WriteAsync(new ClientStreamMethodParam { Par = k });
//     Console.WriteLine($"send par {k}");
//     await Task.Delay(500);
// }
// await clientServerRequestStream.CompleteAsync();
// var clientServerResponse = clientServerStream.ResponseStream;
// while (await clientServerResponse.MoveNext(token))
// {
//     Console.WriteLine($"{ clientServerResponse.Current.Result }");
// }
// ConsoleHelper.WriteInfo("双向流调用 over");
// Console.WriteLine();Console.WriteLine();
//
// ConsoleHelper.WriteInfo("一元复杂调用 start");
// var par = new ServiceMethodParamts
// {
//     Id = 100,
//     Name = "ali yun",
//     IsExists = true,
// };
// par.Roles.Add(new []{"刘备","关羽","张飞"});
// par.Attributes.Add(new Dictionary<string, string>()
// {
//     {"董卓","吕布"},
//     {"王允","貂蝉"}
// });
// par.Detail = Any.Pack(new Person{ Start = Timestamp.FromDateTimeOffset(DateTime.Now)});
// var testResult = await client.ServiceMethodAsync(par);
// if (testResult.ResultCase == ServiceMethodResponse.ResultOneofCase.Person)
// {
//     Console.WriteLine(JsonConvert.SerializeObject(testResult.Person).ToJsonFormatString());
// }
// ConsoleHelper.WriteInfo("一元复杂调用 over");
// Console.WriteLine();Console.WriteLine();
return;

Console.WriteLine("over");

public class Student
{
    public int StdId { get; set; }
    public string StdName { get; set; }
}

public class Result
{
    [JsonProperty("code")] public int Code { get; set; }

    [JsonProperty("msg")] public string Message { get; set; }

    [JsonProperty("data")] public DataModel Data { get; set; }
}

public class DataModel
{
    [JsonProperty("ip")] public string Ip { get; set; }

    [JsonProperty("country")] public string Country { get; set; }

    [JsonProperty("province")] public string Province { get; set; }
}