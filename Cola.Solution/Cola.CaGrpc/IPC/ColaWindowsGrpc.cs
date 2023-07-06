using Cola.CaUtils.Helper;
using Grpc.Core;
using Grpc.Net.Client;
using Microsoft.Extensions.Configuration;

namespace Cola.CaGrpc.IPC;

public class ColaWindowsGrpc : IColaGrpc
{
    public GrpcChannel CreateChannel(string grpcUrl, IConfiguration config, string? pipeNameOrSocketPath=null, CallCredentials? credentials=null)
    {
        if (SystemHelper.NetCoreVersion!.Major > 8)
        {
            var connectionFactory = new NamedPipesConnectionFactory(pipeNameOrSocketPath!);
            var socketsHttpHandler = new SocketsHttpHandler
            {
                ConnectCallback = connectionFactory.ConnectAsync
            };
            var options = new GrpcChannelOptions
            {
                HttpHandler = socketsHttpHandler
            };
            return GrpcChannel.ForAddress(grpcUrl, options.CreateGrpcClientChannelOptions(config,credentials));
        }
        return GrpcChannel.ForAddress(grpcUrl,  new GrpcChannelOptions().CreateGrpcClientChannelOptions(config,credentials));
    }
}