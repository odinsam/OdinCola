using Cola.CaLog.Core;
using Grpc.Core;
using Grpc.Core.Interceptors;

namespace ConsoleApp1Test;

public class ErrorHandlerInterceptor : Interceptor
{
    private readonly IColaLogs _colaLog;
    
    public ErrorHandlerInterceptor(IColaLogs colaLog)
    {
        _colaLog = colaLog;
    }
    public override AsyncUnaryCall<TResponse> AsyncUnaryCall<TRequest, TResponse>(
        TRequest request,
        ClientInterceptorContext<TRequest, TResponse> context,
        AsyncUnaryCallContinuation<TRequest, TResponse> continuation)
    {
        var call = continuation(request, context);
        return new AsyncUnaryCall<TResponse>(
            HandleResponse(call.ResponseAsync),
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
}