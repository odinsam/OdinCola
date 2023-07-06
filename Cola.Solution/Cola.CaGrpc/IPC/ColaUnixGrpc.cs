using System.Net.Sockets;
using Grpc.Core;
using Grpc.Net.Client;
using Microsoft.Extensions.Configuration;

namespace Cola.CaGrpc.IPC;

public class ColaUnixGrpc : IColaGrpc
{
    public GrpcChannel CreateChannel(string grpcUrl, IConfiguration config, string? pipeNameOrSocketPath=null, CallCredentials? credentials=null)
    {
        var udsEndPoint = new UnixDomainSocketEndPoint(pipeNameOrSocketPath!);
        var connectionFactory = new UnixDomainSocketsConnectionFactory(udsEndPoint);
        var socketsHttpHandler = new SocketsHttpHandler
        {
            ConnectCallback = connectionFactory.ConnectAsync
        };
        var options = new GrpcChannelOptions
        {
            HttpHandler = socketsHttpHandler,
        };
        return GrpcChannel.ForAddress(grpcUrl, options.CreateGrpcClientChannelOptions(config,credentials));
    }
}