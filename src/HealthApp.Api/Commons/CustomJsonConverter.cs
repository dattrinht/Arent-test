using System.Text.Json;
using System.Text.Json.Serialization;

namespace HealthApp.Api.Commons;

public sealed class Int64ToStringConverter : JsonConverter<long>
{
    public override long Read(ref Utf8JsonReader r, Type t, JsonSerializerOptions o)
        => long.Parse(r.GetString()!);

    public override void Write(Utf8JsonWriter w, long value, JsonSerializerOptions o)
        => w.WriteStringValue(value.ToString());
}

public sealed class NullableInt64ToStringConverter : JsonConverter<long?>
{
    public override long? Read(ref Utf8JsonReader r, Type t, JsonSerializerOptions o)
    {
        return r.TokenType switch
        {
            JsonTokenType.String => long.Parse(r.GetString()!),
            JsonTokenType.Number => r.GetInt64(),
            _ => null
        };
    }

    public override void Write(Utf8JsonWriter w, long? value, JsonSerializerOptions o)
    {
        if (value is null)
            w.WriteNullValue();
        else
            w.WriteStringValue(value.Value.ToString());
    }
}