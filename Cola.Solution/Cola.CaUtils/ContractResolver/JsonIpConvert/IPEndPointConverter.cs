using System.Net;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Cola.CaUtils.ContractResolver.JsonIpConvert;

public class IpEndPointConverter : JsonConverter
{
    public override bool CanConvert(Type objectType)
    {
        return objectType == typeof(IPEndPoint);
    }

    public override void WriteJson(JsonWriter writer, object? value, JsonSerializer serializer)
    {
        var ep = (IPEndPoint)value!;
        writer.WriteStartObject();
        writer.WritePropertyName("Address");
        serializer.Serialize(writer, ep.Address);
        writer.WritePropertyName("Port");
        writer.WriteValue(ep.Port);
        writer.WriteEndObject();
    }

    public override object ReadJson(JsonReader reader, Type objectType, object? existingValue,
        JsonSerializer serializer)
    {
        var jo = JObject.Load(reader);
        var address = jo["Address"]?.ToObject<IPAddress>(serializer);
        var port = (jo["Port"] ?? throw new InvalidOperationException()).Value<int>();
        return new IPEndPoint(address ?? throw new InvalidOperationException(), port);
    }
}