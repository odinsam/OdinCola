namespace Cola.Models.Core.Models.ColaGrpc;

/// <summary>
/// Grpc Server配置
/// </summary>
public class GrpcServerOption
{
    /// <summary>
    /// 可以从服务器发送的最大消息大小 单位MB
    /// </summary>
    public int? MaxSendMessageSize { get; set; } = null;
    
    /// <summary>
    /// 可以由服务器接收的最大消息大小 单位MB
    /// </summary>
    public int? MaxReceiveMessageSize { get; set; } = 4;

    /// <summary>
    /// 如果为 true，则当服务方法中引发异常时，会将详细异常消息返回到客户端  设置为 true 可能会泄漏敏感信息
    /// </summary>
    public bool EnableDetailedErrors { get; set; } = false;

    /// <summary>
    /// 如果为 true，则对未知服务和方法的调用不会返回 UNIMPLEMENTED 状态，并且请求会传递到 ASP.NET Core 中的下一个注册中间件
    /// </summary>
    public bool IgnoreUnknownServices { get; set; } = false;
}