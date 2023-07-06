using Cola.CaLog.Core;
using Cola.CaUtils.Constants;
using Cola.Models.Core.Models.ColaGrpc;
using Grpc.Core;
using Grpc.Core.Interceptors;
using Microsoft.Extensions.Configuration;

namespace Cola.CaGrpc.ColaGrpcClientInterceptor;

/// <summary>
/// 客户端基本拦截器
/// </summary>
public class ClientGrpcInterceptor : Interceptor
{
    private readonly IColaLogs _colaLog;
    private readonly IConfiguration _config;
    public ClientGrpcInterceptor( 
        IColaLogs colaLog, 
        IConfiguration config)
    {
        _colaLog = colaLog;
        _config = config;
    }

    /// <summary>
    /// 截获一元 RPC 异步调用
    /// </summary>
    /// <param name="request"></param>
    /// <param name="context"></param>
    /// <param name="continuation"></param>
    /// <typeparam name="TRequest"></typeparam>
    /// <typeparam name="TResponse"></typeparam>
    /// <returns></returns>
    public override AsyncUnaryCall<TResponse> AsyncUnaryCall<TRequest, TResponse>(
        TRequest request,
        ClientInterceptorContext<TRequest, TResponse> context,
        AsyncUnaryCallContinuation<TRequest, TResponse> continuation)
    {
        ClientInterceptorLog($" gRpc调用类型: 一元 RPC 异步调用. \r\n gRpc调用方法: {context.Method.Name}.",context);
        var call = continuation(request, context);
        return new AsyncUnaryCall<TResponse>(
            HandleResponse(call.ResponseAsync),
            call.ResponseHeadersAsync,
            call.GetStatus,
            call.GetTrailers,
            call.Dispose);
    }

    /// <summary>
    /// 截获一元 RPC 阻塞调用
    /// </summary>
    /// <param name="request"></param>
    /// <param name="context"></param>
    /// <param name="continuation"></param>
    /// <typeparam name="TRequest"></typeparam>
    /// <typeparam name="TResponse"></typeparam>
    /// <returns></returns>
    public override TResponse BlockingUnaryCall<TRequest, TResponse>(TRequest request, ClientInterceptorContext<TRequest, TResponse> context,
        BlockingUnaryCallContinuation<TRequest, TResponse> continuation) 
    {
        ClientInterceptorLog($" gRpc调用类型: 一元 RPC 阻塞调用. \r\n gRpc调用方法: {context.Method.Name}.",context);
        return continuation(request, context);
    }

    /// <summary>
    /// 截获客户端流式处理 RPC 异步调用
    /// </summary>
    /// <param name="context"></param>
    /// <param name="continuation"></param>
    /// <typeparam name="TRequest"></typeparam>
    /// <typeparam name="TResponse"></typeparam>
    /// <returns></returns>
    public override AsyncClientStreamingCall<TRequest, TResponse> AsyncClientStreamingCall<TRequest, TResponse>(ClientInterceptorContext<TRequest, TResponse> context,
        AsyncClientStreamingCallContinuation<TRequest, TResponse> continuation)
    {
        ClientInterceptorLog($" gRpc调用类型: 客户端流式处理 RPC 异步调用. \r\n gRpc调用方法: {context.Method.Name}.",context);
        var call = continuation(context);
        return new AsyncClientStreamingCall<TRequest, TResponse>(
            HandleResponse(call.RequestStream),
            HandleResponse(call.ResponseAsync),
            call.ResponseHeadersAsync,
            call.GetStatus,
            call.GetTrailers,
            call.Dispose);
    }
    /// <summary>
    /// 截获服务器流式处理 RPC 异步调用
    /// </summary>
    /// <param name="request"></param>
    /// <param name="context"></param>
    /// <param name="continuation"></param>
    /// <typeparam name="TRequest"></typeparam>
    /// <typeparam name="TResponse"></typeparam>
    /// <returns></returns>

    public override AsyncServerStreamingCall<TResponse> AsyncServerStreamingCall<TRequest, TResponse>(TRequest request,
        ClientInterceptorContext<TRequest, TResponse> context, AsyncServerStreamingCallContinuation<TRequest, TResponse> continuation)
    {
        ClientInterceptorLog($" gRpc调用类型: 服务器流式处理 RPC 异步调用. \r\n gRpc调用方法: {context.Method.Name}.",context);
        var call = continuation(request,context);
        return new AsyncServerStreamingCall<TResponse>(
            HandleResponse(call.ResponseStream),
            call.ResponseHeadersAsync,
            call.GetStatus,
            call.GetTrailers,
            call.Dispose);
    }

    /// <summary>
    /// 截获双向流式处理 RPC 异步调用
    /// </summary>
    /// <param name="context"></param>
    /// <param name="continuation"></param>
    /// <typeparam name="TRequest"></typeparam>
    /// <typeparam name="TResponse"></typeparam>
    /// <returns></returns>
    public override AsyncDuplexStreamingCall<TRequest, TResponse> AsyncDuplexStreamingCall<TRequest, TResponse>(ClientInterceptorContext<TRequest, TResponse> context,
        AsyncDuplexStreamingCallContinuation<TRequest, TResponse> continuation)
    {
        ClientInterceptorLog($" gRpc调用类型: 双向流式处理 RPC 异步调用. \r\n gRpc调用方法: {context.Method.Name}.",context);
        var call = continuation(context);
        return new AsyncDuplexStreamingCall<TRequest, TResponse>(
            HandleResponse(call.RequestStream),
            HandleResponse(call.ResponseStream),
            call.ResponseHeadersAsync,
            call.GetStatus,
            call.GetTrailers,
            call.Dispose);
    }

    private async Task<TResponse> HandleResponse<TResponse>(Task<TResponse> inner)
    {
        try
        {
            return await inner;
        }
        catch (Exception ex)
        {
            _colaLog.Error(ex);
            throw new InvalidOperationException("Custom error", ex);
        }
    }
    
    private TResponse HandleResponse<TResponse>(TResponse inner)
    {
        try
        {
            return inner;
        }
        catch (Exception ex)
        {
            _colaLog.Error(ex);
            throw new InvalidOperationException("Custom error", ex);
        }
    }
    
    /// <summary>
    /// 私有方法记录日志
    /// </summary>
    /// <param name="logInfo"></param>
    /// <param name="context"></param>
    /// <typeparam name="TRequest"></typeparam>
    /// <typeparam name="TResponse"></typeparam>
    private void ClientInterceptorLog<TRequest, TResponse>(string logInfo,ClientInterceptorContext<TRequest, TResponse> context) where TRequest:class where TResponse:class
    {
        var interceptorLog = _config.GetSection(SystemConstant.CONSTANT_COLAGRPCCLIENT_SECTION).Get<GrpcClientOption>()
            .InterceptorLog;
        if (interceptorLog)
        {
            _colaLog.Info(logInfo);
        }
    }
}