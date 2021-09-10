using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace TinyHelpers.Json.Serialization
{
    public class UtcDateTimeConverter : JsonConverter<DateTime>
    {
        private readonly string serializationFormat;

        public UtcDateTimeConverter() : this(null)
        {
        }

        public UtcDateTimeConverter(string? serializationFormat)
            => this.serializationFormat = serializationFormat ?? "yyyy'-'MM'-'dd'T'HH':'mm':'ssZ";

        public override DateTime Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            var value = reader.GetString();
            if (value == null)
            {
#pragma warning disable CA2208 // Instantiate argument exceptions correctly
                throw new ArgumentNullException(nameof(value));
#pragma warning restore CA2208 // Instantiate argument exceptions correctly
            }

            return DateTime.Parse(value).ToUniversalTime();
        }

        public override void Write(Utf8JsonWriter writer, DateTime value, JsonSerializerOptions options)
            => writer.WriteStringValue((value.Kind == DateTimeKind.Local ? value.ToUniversalTime() : value)
                .ToString(serializationFormat));
    }
}
