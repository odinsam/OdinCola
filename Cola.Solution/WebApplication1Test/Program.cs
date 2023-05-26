using System.Net;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using WebApplication1Test;

var builder = WebApplication.CreateBuilder(args);

var config = builder.Configuration;
// Add services to the container.
// builder.WebHost.UseUrls("http://localhost:5400");
builder.Services.AddControllers();
builder.Services.AddMemoryCache();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle

builder.Services.AddEndpointsApiExplorer();
//将Redis分布式缓存服务添加到服务中
builder.Services.AddDistributedRedisCache(options =>
{
    //用于连接Redis的配置  
    options.Configuration =
        "47.122.0.223:6379,password=173djjDJJ,defaultDatabase=0,connectTimeout=5000,syncTimeout=1000"; // Configuration.GetConnectionString("RedisConnectionString");
    //Redis实例名RedisDistributedCache
    options.InstanceName = "RedisDistributedCache";
});
builder.Services.AddSwaggerGen();
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
builder.Services.AddGrpc();
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
app.UseAuthorization();
app.UseEndpoints(endpoints =>
{
    endpoints.MapGrpcService<GreeterService>();
    endpoints.MapControllers();
});
// app.MapGrpcService<GreeterService>();
app.Run();