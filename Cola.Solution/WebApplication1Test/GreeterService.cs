using Google.Protobuf.Collections;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using Newtonsoft.Json;
using WebApi.Protos;

namespace WebApplication1Test;

public class GreeterService : Greeter.GreeterBase
{
    private readonly ILogger<GreeterService> _logger;
    public GreeterService(ILogger<GreeterService> logger)
    {
        _logger = logger;
    }
    
    public override Task<HelloReply> SayHello(HelloRequest request, ServerCallContext context)
    {
        foreach (var item in context.RequestHeaders)
        {
            Console.WriteLine($"item key:{item.Key}   item value:{item.Value}");
        }
        return Task.FromResult(new HelloReply
        {
            Message = "Hello webApi " + request.Name
        });
    }

    public override Task<ServiceMethodResponse> ServiceMethod(ServiceMethodParamts request, ServerCallContext context)
    {
        var response = new ServiceMethodResponse()
        {
            Person = new Person
            {
                Start = Timestamp.FromDateTimeOffset(DateTime.Now),
            }
        };
        Dictionary<string, string> dic = new Dictionary<string, string>()
        {
            {"Id",request.Id.ToString()},
            {"Name",request.Name},
            {"IsExists",request.IsExists.ToString()},
            {"Roles",JsonConvert.SerializeObject(request.Roles)},
            {"Attributes",JsonConvert.SerializeObject(request.Attributes)},
            {"Detail", JsonConvert.SerializeObject(request.Detail.Unpack<Person>())}
        };
        response.Person.Attributes.Add(dic);
        // int a = 1, b = 0;
        // int c = a / b;
        return Task.FromResult(response);
    }
    
    /// <summary>
    /// 客户端流实现
    /// </summary>
    /// <param name="requestStream"></param>
    /// <param name="context"></param>
    /// <returns></returns>
    public override async Task<ClientStreamMethodResponse> ClientStreamMethodAsync(IAsyncStreamReader<ClientStreamMethodParam> requestStream, ServerCallContext context)
    {
        int result = 0;
        while (await requestStream.MoveNext())
        {
            result += requestStream.Current.Par;
            Console.WriteLine($"Current Result { result }");
        }
        return await Task.FromResult(new ClientStreamMethodResponse() { Result = result });
    }

    /// <summary>
    /// 服务器流
    /// </summary>
    /// <param name="request"></param>
    /// <param name="responseStream"></param>
    /// <param name="context"></param>
    /// <returns></returns>
    public override async Task ServerStreamMethodAsync(ClientMethodParam request, IServerStreamWriter<ServerStreamMethodResponse> responseStream, ServerCallContext context)
    {
        foreach (var item in request.Lst)
        {
            await responseStream.WriteAsync(new ServerStreamMethodResponse { Result = $"[{DateTime.Now:yyyy-M-d hh:mm:ss}] { item+100 } " });
            await Task.Delay(1000);
        }
        Console.WriteLine("server response over");
    }

    /// <summary>
    /// 双向流
    /// </summary>
    /// <param name="requestStream"></param>
    /// <param name="responseStream"></param>
    /// <param name="context"></param>
    public override async Task ClientServerStreamMethodAsync(IAsyncStreamReader<ClientStreamMethodParam> requestStream, IServerStreamWriter<ServerStreamMethodResponse> responseStream,
        ServerCallContext context)
    {
        var lst = new List<int>();
        while (await requestStream.MoveNext())
        {
            var result = requestStream.Current.Par;
            lst.Add(result);
        }

        foreach (var item in lst)
        {
            await responseStream.WriteAsync(new ServerStreamMethodResponse { Result = $"[{DateTime.Now:yyyy-M-d hh:mm:ss}] response stream result: { item } " });
            await Task.Delay(500);
        }
    }
}
