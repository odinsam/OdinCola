using System.Net;
using Cola.CaCache;
using Cola.CaException;
using Cola.CaGrpc;
using Cola.CaJwt;
using Cola.CaLog;
using Cola.CaSnowFlake;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using WebApplication1Test;

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
// Add services to the container.
// builder.WebHost.UseUrls("http://localhost:5400");
//jwt注入
builder.Services.AddColaJwt<ApiResponseForAuthenticationHandler>(config);
builder.Services.AddControllers(options =>
{
    options.Filters.Add<ColaAsyncAuthorizationFilter>();
    options.Filters.Add<ColaIAlwaysRunResultFilter>();
});
builder.Services.AddMemoryCache();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddColaGrpc(config);
builder.Services.AddEndpointsApiExplorer();
//将Redis分布式缓存服务添加到服务中
// 缓存 组件
builder.Services.AddSingletonColaCache(config);
builder.Services.AddColaSwaggerGen();
//允许一个或多个来源可以跨域
builder.Services.AddCors(options =>
{
    options.AddPolicy("cors", policy =>
    {
        // 设定允许跨域的来源，有多个可以用','隔开
        policy
            .AllowAnyOrigin()
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});
builder.WebHost.ConfigureKestrel(options =>
{
    if (OperatingSystem.IsWindows())
    {
        //NET8 可以使用 IPC 代替
        //serverOptions.ListenNamedPipe("MyPipeName");
        options.Listen(IPAddress.Parse("127.0.0.1"),5004, listenOptions =>
        {
            //http调用grpc
            listenOptions.Protocols = HttpProtocols.Http2;
        });
        options.Listen(IPAddress.Parse("127.0.0.1"),5005, listenOptions =>
        {
            //https调用grpc 
            listenOptions.Protocols = HttpProtocols.Http1AndHttp2;
            listenOptions.UseHttps();
        });
    }
    else
    {
        var socketPath = Path.Combine(Path.GetTempPath(), "socket.tmp");
        options.ListenUnixSocket(socketPath);
        options.ConfigureEndpointDefaults(listenOptions =>
        {
            listenOptions.Protocols = HttpProtocols.Http2;
        });
    }
});




var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.MapControllers();
app.UseCors("cors");
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
app.UseEndpoints(endpoints =>
{
    endpoints.MapGrpcService<GreeterService>();
    endpoints.MapControllers();
});
// app.MapGrpcService<GreeterService>();
app.Run();