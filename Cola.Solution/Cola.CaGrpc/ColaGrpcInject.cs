using Cola.CaConsole;
using Cola.CaGrpc.ColaGrpcClientInterceptor;
using Cola.CaLog.Core;
using Cola.CaUtils.Constants;
using Cola.Models.Core.Models.ColaGrpc;
using Grpc.AspNetCore.Server;
using Grpc.Core;
using Grpc.Core.Interceptors;
using Grpc.Net.Client;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Cola.CaGrpc;

public static class ColaGrpcInject
{
    /// <summary>
    /// 全局grpc服务注入
    /// </summary>
    /// <param name="services"></param>
    /// <param name="config"></param>
    /// <returns></returns>
    public static IGrpcServerBuilder AddColaGrpc(
        this IServiceCollection services,
        IConfiguration config)
    {
        var grpcServerOption = config.GetSection(SystemConstant.CONSTANT_COLAGRPCSERVER_SECTION).Get<GrpcServerOption>();
        grpcServerOption = grpcServerOption ?? new GrpcServerOption();
        return InjectGrpcServerGlobal(services, config, grpcServerOption);
    }
    
    /// <summary>
    /// 单独的grpc服务注入
    /// </summary>
    /// <param name="serverBuilder"></param>
    /// <param name="config"></param>
    /// <typeparam name="TService"></typeparam>
    /// <returns></returns>
    public static IGrpcServerBuilder AddColaSingleServerGrpc<TService>(this IGrpcServerBuilder serverBuilder, IConfiguration config) where TService : class
    {
        var serverName = typeof(TService).Name;
        var serverOption = config.GetSection($"{SystemConstant.CONSTANT_COLAGRPCSERVER_SECTION}:{serverName}").Get<GrpcServerOption>();
        return serverBuilder.AddServiceOptions<TService>(options =>
        {
            options.EnableDetailedErrors = serverOption.EnableDetailedErrors;
            options.MaxReceiveMessageSize = serverOption.MaxReceiveMessageSize!=null? serverOption.MaxReceiveMessageSize * 1024 * 1024:serverOption.MaxReceiveMessageSize;
            options.MaxSendMessageSize = serverOption.MaxSendMessageSize!=null?serverOption.MaxSendMessageSize * 1024 * 1024:serverOption.MaxSendMessageSize;
            options.IgnoreUnknownServices = serverOption.IgnoreUnknownServices;
        });
    }
    
    /// <summary>
    /// 单独的grpc服务注入
    /// </summary>
    /// <param name="serverBuilder"></param>
    /// <param name="config"></param>
    /// <typeparam name="TService"></typeparam>
    /// <typeparam name="TSingleInterceptor"></typeparam>
    /// <returns></returns>
    public static IGrpcServerBuilder AddColaSingleServerGrpc<TService,TSingleInterceptor>(this IGrpcServerBuilder serverBuilder, IConfiguration config) where TService : class where TSingleInterceptor: Interceptor
    {
        var serverName = typeof(TService).Name;
        var serverOption = config.GetSection($"{SystemConstant.CONSTANT_COLAGRPCSERVER_SECTION}:{serverName}").Get<GrpcServerOption>();
        return serverBuilder.AddServiceOptions<TService>(options =>
        {
            options.EnableDetailedErrors = serverOption.EnableDetailedErrors;
            options.MaxReceiveMessageSize = serverOption.MaxReceiveMessageSize!=null? serverOption.MaxReceiveMessageSize * 1024 * 1024:serverOption.MaxReceiveMessageSize;
            options.MaxSendMessageSize = serverOption.MaxSendMessageSize!=null?serverOption.MaxSendMessageSize * 1024 * 1024:serverOption.MaxSendMessageSize;
            options.IgnoreUnknownServices = serverOption.IgnoreUnknownServices;
            options.Interceptors.Add<TSingleInterceptor>();
        });
    }
    
    /// <summary>
    /// 单独的grpc服务注入
    /// </summary>
    /// <param name="serverBuilder"></param>
    /// <param name="action"></param>
    /// <typeparam name="TService"></typeparam>
    /// <returns></returns>
    public static IGrpcServerBuilder AddColaSingleServerGrpc<TService>(this IGrpcServerBuilder serverBuilder, Action<GrpcServerOption> action) where TService : class
    {
        var grpcServerOption = new GrpcServerOption();
        action(grpcServerOption);
        return serverBuilder.AddServiceOptions<TService>(options =>
        {
            options.EnableDetailedErrors = grpcServerOption.EnableDetailedErrors;
            options.MaxReceiveMessageSize = grpcServerOption.MaxReceiveMessageSize!=null? grpcServerOption.MaxReceiveMessageSize * 1024 * 1024:grpcServerOption.MaxReceiveMessageSize;
            options.MaxSendMessageSize = grpcServerOption.MaxSendMessageSize!=null?grpcServerOption.MaxSendMessageSize * 1024 * 1024:grpcServerOption.MaxSendMessageSize;
            options.IgnoreUnknownServices = grpcServerOption.IgnoreUnknownServices;
        });
    }
    
    /// <summary>
    /// 单独的grpc服务注入
    /// </summary>
    /// <param name="serverBuilder"></param>
    /// <param name="action"></param>
    /// <typeparam name="TService"></typeparam>
    /// <typeparam name="TSingleInterceptor"></typeparam>
    /// <returns></returns>
    public static IGrpcServerBuilder AddColaSingleServerGrpc<TService,TSingleInterceptor>(this IGrpcServerBuilder serverBuilder, Action<GrpcServerOption> action) where TService : class where TSingleInterceptor: Interceptor
    {
        var grpcServerOption = new GrpcServerOption();
        action(grpcServerOption);
        return serverBuilder.AddServiceOptions<TService>(options =>
        {
            options.EnableDetailedErrors = grpcServerOption.EnableDetailedErrors;
            options.MaxReceiveMessageSize = grpcServerOption.MaxReceiveMessageSize!=null? grpcServerOption.MaxReceiveMessageSize * 1024 * 1024:grpcServerOption.MaxReceiveMessageSize;
            options.MaxSendMessageSize = grpcServerOption.MaxSendMessageSize!=null?grpcServerOption.MaxSendMessageSize * 1024 * 1024:grpcServerOption.MaxSendMessageSize;
            options.IgnoreUnknownServices = grpcServerOption.IgnoreUnknownServices;
            options.Interceptors.Add<TSingleInterceptor>();
        });
    }

    public static GrpcChannelOptions CreateGrpcClientChannelOptions(this GrpcChannelOptions options, IConfiguration config, CallCredentials? credentials=null)
    {
        var clientOption = config.GetSection(SystemConstant.CONSTANT_COLAGRPCCLIENT_SECTION).Get<GrpcClientOption>();
        options.DisposeHttpClient = clientOption.DisposeHttpClient;
        options.MaxSendMessageSize = clientOption.MaxSendMessageSize!=null?clientOption.MaxSendMessageSize * 1024*1024:clientOption.MaxSendMessageSize;
        options.MaxReceiveMessageSize = clientOption.MaxReceiveMessageSize!=null?clientOption.MaxReceiveMessageSize * 1024*1024:clientOption.MaxReceiveMessageSize;
        options.ThrowOperationCanceledOnCancellation = clientOption.ThrowOperationCanceledOnCancellation;
        if(credentials!=null)
            options.Credentials = ChannelCredentials.Create(new SslCredentials(),credentials);
        return options;
    }
    
    public static GrpcChannelOptions CreateGrpcClientChannelOptions(this GrpcChannelOptions options, Action<GrpcClientOption> action, CallCredentials? credentials=null)
    {
        var clientOption = new GrpcClientOption();
        action(clientOption);
        options.DisposeHttpClient = clientOption.DisposeHttpClient;
        options.MaxSendMessageSize = clientOption.MaxSendMessageSize;
        options.MaxReceiveMessageSize = clientOption.MaxReceiveMessageSize;
        options.ThrowOperationCanceledOnCancellation = clientOption.ThrowOperationCanceledOnCancellation;
        if(credentials!=null)
            options.Credentials = ChannelCredentials.Create(new SslCredentials(),credentials);
        return options;
    }

    #region 私有方法
    
    private static IGrpcServerBuilder InjectGrpcServerGlobal(IServiceCollection services, IConfiguration config, GrpcServerOption grpcServerOption)
    {
        var grpc = services.AddGrpc(options =>
        {
            options.EnableDetailedErrors = grpcServerOption.EnableDetailedErrors;
            options.MaxReceiveMessageSize = grpcServerOption.MaxReceiveMessageSize!=null? grpcServerOption.MaxReceiveMessageSize * 1024 * 1024:grpcServerOption.MaxReceiveMessageSize;
            options.MaxSendMessageSize = grpcServerOption.MaxSendMessageSize!=null?grpcServerOption.MaxSendMessageSize * 1024 * 1024:grpcServerOption.MaxSendMessageSize;
            options.IgnoreUnknownServices = grpcServerOption.IgnoreUnknownServices;
            options.Interceptors.Add<ServerGrpcInterceptor>(services.BuildServiceProvider().GetService<IColaLogs>()!,config);
        });
        ConsoleHelper.WriteInfo("注入【 Grpc 】");
        return grpc;
    }

    #endregion
}