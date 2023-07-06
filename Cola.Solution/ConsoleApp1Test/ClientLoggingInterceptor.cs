using Cola.CaLog.Core;
using Grpc.Core;
using Grpc.Core.Interceptors;
using Microsoft.Extensions.Logging;

namespace ConsoleApp1Test;

public class ClientLoggingInterceptor : Interceptor
{
    private readonly IColaLogs _colaLog;

    public ClientLoggingInterceptor(IColaLogs colaLog)
    {
        _colaLog = colaLog;
    }

    public override AsyncUnaryCall<TResponse> AsyncUnaryCall<TRequest, TResponse>(
        TRequest request,
        ClientInterceptorContext<TRequest, TResponse> context,
        AsyncUnaryCallContinuation<TRequest, TResponse> continuation)
    {
        _colaLog.Info($"Starting call. Type: {context.Method.Type}. " +
                 $"Method: {context.Method.Name}.");
        return continuation(request, context);
    }
}