using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace TinyHelpers.Json.Serialization
{
    public class UtcDateTimeConverter : JsonConverter<DateTime>
    {
        private readonly string? _format;

        public UtcDateTimeConverter(string? format = null)
        {
            _format = format;
        }

        public override DateTime Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
            => DateTime.Parse(reader.GetString()).ToUniversalTime();

        public override void Write(Utf8JsonWriter writer, DateTime value, JsonSerializerOptions options)
            => writer.WriteStringValue((value.Kind == DateTimeKind.Unspecified ? value : value.ToUniversalTime()).ToString(_format ?? "yyyy'-'MM'-'dd'T'HH':'mm':'ssZ"));
    }
}
