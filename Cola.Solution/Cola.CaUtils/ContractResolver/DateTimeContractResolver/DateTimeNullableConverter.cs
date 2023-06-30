using Newtonsoft.Json;

namespace Cola.CaUtils.ContractResolver.DateTimeContractResolver;

public class DateTimeNullableConverter : JsonConverter<DateTime?>
{
    public DateTimeNullableConverter(string dateTimeType)
    {
        DateTimeType = dateTimeType;
    }

    public string DateTimeType { get; set; }

    public override DateTime? ReadJson(JsonReader reader, Type objectType, DateTime? existingValue,
        bool hasExistingValue, JsonSerializer serializer)
    {
        return string.IsNullOrEmpty(reader.ToString()) ? default(DateTime?) : DateTime.Parse(reader.ToString()!);
    }

    public override void WriteJson(JsonWriter writer, DateTime? value, JsonSerializer serializer)
    {
        writer.WriteValue(value?.ToString(DateTimeType));
    }
}