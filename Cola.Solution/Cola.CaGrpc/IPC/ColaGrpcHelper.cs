using Grpc.Core;
using Grpc.Net.Client;
using Microsoft.Extensions.Configuration;

namespace Cola.CaGrpc.IPC;

public class ColaGrpcHelper
{
    private readonly IColaGrpc _colaGrpc;
    
    public ColaGrpcHelper(IColaGrpc colaGrpc)
    {
        _colaGrpc = colaGrpc;
    }
    
    public GrpcChannel CreateChannel(string grpcUrl, IConfiguration config, string? pipeNameOrSocketPath=null, CallCredentials? credentials=null)
    {
        return _colaGrpc.CreateChannel(grpcUrl, config, pipeNameOrSocketPath,credentials);
    }
}