using Grpc.Core;
using Grpc.Net.Client;
using Microsoft.Extensions.Configuration;

namespace Cola.CaGrpc.IPC;

public interface IColaGrpc
{
    GrpcChannel CreateChannel(string grpcUrl, IConfiguration config, string? pipeNameOrSocketPath=null, CallCredentials? credentials=null);
}