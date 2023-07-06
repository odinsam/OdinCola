namespace Cola.Models.Core.Models.ColaGrpc;

public class GrpcClientOption
{
    /// <summary>
    /// 如果设置为 true 且指定了 HttpMessageHandler 或 HttpClient，则在处置 GrpcChannel 时，将分别处置 HttpHandler 或 HttpClient
    /// </summary>
    public bool DisposeHttpClient { get; set; } = false;
    
    /// <summary>
    /// 可以从客户端发送的最大消息大小 单位MB
    /// </summary>
    public int? MaxSendMessageSize { get; set; } = null;
    
    /// <summary>
    /// 可以由客户端接收的最大消息大小 单位MB
    /// </summary>
    public int? MaxReceiveMessageSize { get; set; } = 4;

    /// <summary>
    /// 如果设置为 true，则在取消调用或超过其截止时间时，客户端将引发 OperationCanceledException
    /// </summary>
    public bool ThrowOperationCanceledOnCancellation { get; set; } = false;

    /// <summary>
    /// 拦截器日志 默认开启
    /// </summary>
    public bool InterceptorLog { get; set; } = true;
}