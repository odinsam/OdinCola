using System.IO.Pipes;
using System.Reflection;
using System.Security.Principal;
using Cola.CaUtils.Extensions;
using Cola.CaUtils.Helper;

namespace ConsoleApp1Test;

/// <summary>
/// Grpc IPC进程内通信 只支持 .NET 8以上的版本(含)
/// </summary>
public class NamedPipesConnectionFactory
{
    private readonly string _pipeName;

    public NamedPipesConnectionFactory(string pipeName)
    {
        _pipeName = pipeName;
    }

    public async ValueTask<Stream> ConnectAsync(SocketsHttpConnectionContext _,
        CancellationToken cancellationToken = default)
    {
        var clientStream = new NamedPipeClientStream(
            serverName: ".",
            pipeName: _pipeName,
            direction: PipeDirection.InOut,
            options: PipeOptions.WriteThrough | PipeOptions.Asynchronous,
            impersonationLevel: TokenImpersonationLevel.Anonymous);
        try
        {
            await clientStream.ConnectAsync(cancellationToken).ConfigureAwait(false);
            return clientStream;
        }
        catch
        {
            clientStream.Dispose();
            throw;
        }
    }
}