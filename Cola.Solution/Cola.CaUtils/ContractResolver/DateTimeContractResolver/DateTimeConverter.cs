using Newtonsoft.Json;

namespace Cola.CaUtils.ContractResolver.DateTimeContractResolver;

public class DateTimeConverter : JsonConverter<DateTime>
{
    public DateTimeConverter(string dateTimeType)
    {
        DateTimeType = dateTimeType;
    }

    public string DateTimeType { get; set; }

    public override DateTime ReadJson(JsonReader reader, Type objectType, DateTime existingValue, bool hasExistingValue,
        JsonSerializer serializer)
    {
        return DateTime.Parse(reader.ToString()!);
    }

    public override void WriteJson(JsonWriter writer, DateTime value, JsonSerializer serializer)
    {
        writer.WriteValue(value.ToString(DateTimeType));
    }
}