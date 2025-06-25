using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace ProjectTSSI.Models;

public class ObjectToInferredTypeConverter : JsonConverter<object>
{
    public override object Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        => JsonDocument.ParseValue(ref reader).RootElement.Clone();

    public override void Write(Utf8JsonWriter writer, object value, JsonSerializerOptions options)
    {
        JsonSerializer.Serialize(writer, value, value.GetType(), options);
    }
}
