using Cola.CaLog.Core;
using Cola.CaUtils.Constants;
using Cola.Models.Core.Models.ColaGrpc;
using Grpc.Core;
using Grpc.Core.Interceptors;
using Microsoft.Extensions.Configuration;

namespace Cola.CaGrpc.ColaGrpcClientInterceptor;

/// <summary>
/// 服务端基本拦截器
/// </summary>
public class ServerGrpcInterceptor : Interceptor
{
    private readonly IColaLogs _colaLog;
    private readonly IConfiguration _config;
    public ServerGrpcInterceptor( 
        IColaLogs colaLog, 
        IConfiguration config)
    {
        _colaLog = colaLog;
        _config = config;
    }

    /// <summary>
    /// 截获一元 RPC
    /// </summary>
    /// <param name="request"></param>
    /// <param name="context"></param>
    /// <param name="continuation"></param>
    /// <typeparam name="TRequest"></typeparam>
    /// <typeparam name="TResponse"></typeparam>
    /// <returns></returns>
    public override async Task<TResponse> UnaryServerHandler<TRequest, TResponse>(TRequest request, ServerCallContext context,
        UnaryServerMethod<TRequest, TResponse> continuation)
    {
        ClientInterceptorLog($" gRpc类型: 一元 RPC 异步方法. \r\n gRpc方法: {context.Method}.");
        try
        {
            return await continuation(request, context);
        }
        catch (Exception ex)
        {
            _colaLog.Error(ex);
            throw;
        }
    }


    /// <summary>
    /// 截获客户端流式处理 RPC
    /// </summary>
    /// <param name="requestStream"></param>
    /// <param name="context"></param>
    /// <param name="continuation"></param>
    /// <typeparam name="TRequest"></typeparam>
    /// <typeparam name="TResponse"></typeparam>
    /// <returns></returns>
    public override async Task<TResponse> ClientStreamingServerHandler<TRequest, TResponse>(IAsyncStreamReader<TRequest> requestStream, ServerCallContext context,
        ClientStreamingServerMethod<TRequest, TResponse> continuation)
    {
        ClientInterceptorLog($" gRpc类型: 客户端流 RPC 异步方法. \r\n gRpc方法: {context.Method}.");
        try
        {
            return await continuation(requestStream,context);
        }
        catch (Exception ex)
        {
            _colaLog.Error(ex);
            throw;
        }
    }

    /// <summary>
    /// 截获服务器流式处理 RPC
    /// </summary>
    /// <param name="request"></param>
    /// <param name="responseStream"></param>
    /// <param name="context"></param>
    /// <param name="continuation"></param>
    /// <typeparam name="TRequest"></typeparam>
    /// <typeparam name="TResponse"></typeparam>
    public override async Task ServerStreamingServerHandler<TRequest, TResponse>(TRequest request, IServerStreamWriter<TResponse> responseStream,
        ServerCallContext context, ServerStreamingServerMethod<TRequest, TResponse> continuation)
    {
        ClientInterceptorLog($" gRpc类型: 服务器流 RPC 异步方法. \r\n gRpc方法: {context.Method}.");
        try
        {
            await continuation(request, responseStream, context);
        }
        catch (Exception ex)
        {
            _colaLog.Error(ex);
            throw;
        }
    }

    /// <summary>
    /// 截获双向流式处理 RPC
    /// </summary>
    /// <param name="requestStream"></param>
    /// <param name="responseStream"></param>
    /// <param name="context"></param>
    /// <param name="continuation"></param>
    /// <typeparam name="TRequest"></typeparam>
    /// <typeparam name="TResponse"></typeparam>
    /// <returns></returns>
    public override async Task DuplexStreamingServerHandler<TRequest, TResponse>(IAsyncStreamReader<TRequest> requestStream,
        IServerStreamWriter<TResponse> responseStream, ServerCallContext context, DuplexStreamingServerMethod<TRequest, TResponse> continuation)
    {
        ClientInterceptorLog($" gRpc类型: 双向流 RPC 异步方法. \r\n gRpc方法: {context.Method}.");
        try
        {
            await continuation(requestStream, responseStream, context);
        }
        catch (Exception ex)
        {
            _colaLog.Error(ex);
            throw;
        }
    }


    /// <summary>
    /// 私有方法记录日志
    /// </summary>
    /// <param name="logInfo"></param>
    private void ClientInterceptorLog(string logInfo)
    {
        var interceptorLog = _config.GetSection(SystemConstant.CONSTANT_COLAGRPCSERVER_SECTION).Get<GrpcClientOption>()
            .InterceptorLog;
        if (interceptorLog)
        {
            _colaLog.Info(logInfo);
        }
    }
}