using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace TinyHelpers.Json.Serialization
{
    public class TimeSpanConverter : JsonConverter<TimeSpan>
    {
        public override TimeSpan Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
            => TimeSpan.Parse(reader.GetString());

        public override void Write(Utf8JsonWriter writer, TimeSpan value, JsonSerializerOptions options)
            => writer.WriteStringValue(value.ToString());

        public void Write(Utf8JsonWriter writer, TimeSpan value, JsonSerializerOptions options, string? format = null)
            => writer.WriteStringValue(value.ToString(format ?? string.Empty));
    }
}
