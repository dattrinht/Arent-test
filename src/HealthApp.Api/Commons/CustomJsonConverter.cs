using System.Text.Json;
using System.Text.Json.Serialization;

namespace HealthApp.Api.Commons;

public sealed class Int64ToStringConverter : JsonConverter<long>
{
    public override long Read(ref Utf8JsonReader r, Type t, JsonSerializerOptions o)
    {
        return r.TokenType switch
        {
            JsonTokenType.String => long.Parse(r.GetString()!, provider: null),
            JsonTokenType.Number => r.GetInt64(),
            JsonTokenType.Null => throw new JsonException("Expected number or string for Int64."),
            _ => throw new JsonException($"Unexpected token {r.TokenType} for Int64.")
        };
    }

    public override void Write(Utf8JsonWriter w, long value, JsonSerializerOptions o)
        => w.WriteStringValue(value.ToString());
}

public sealed class NullableInt64ToStringConverter : JsonConverter<long?>
{
    public override long? Read(ref Utf8JsonReader r, Type t, JsonSerializerOptions o)
    {
        return r.TokenType switch
        {
            JsonTokenType.String => string.IsNullOrWhiteSpace(r.GetString()) ? (long?)null : long.Parse(r.GetString()!, provider: null),
            JsonTokenType.Number => r.GetInt64(),
            JsonTokenType.Null => null,
            _ => throw new JsonException($"Unexpected token {r.TokenType} for nullable Int64.")
        };
    }

    public override void Write(Utf8JsonWriter w, long? value, JsonSerializerOptions o)
    {
        if (value is null) { w.WriteNullValue(); return; }
        w.WriteStringValue(value.Value.ToString());
    }
}