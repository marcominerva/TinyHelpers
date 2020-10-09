using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace TinyNetHelpers.Json.Serialization
{
    public class UtcDateTimeConverter : JsonConverter<DateTime>
    {
        public override DateTime Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
            => DateTime.Parse(reader.GetString()).ToUniversalTime();

        public override void Write(Utf8JsonWriter writer, DateTime value, JsonSerializerOptions options)
            => writer.WriteStringValue((value.Kind == DateTimeKind.Unspecified ? value : value.ToUniversalTime()).ToString("yyyy'-'MM'-'dd'T'HH':'mm':'ssZ"));
    }
}
