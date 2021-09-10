using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace TinyHelpers.Json.Serialization
{
#if NETSTANDARD2_0
    public class TimeSpanConverter : JsonConverter<TimeSpan>
    {
        private readonly string serializationFormat;

        public TimeSpanConverter() : this(null)
        {
        }

        public TimeSpanConverter(string? serializationFormat)
            => this.serializationFormat = serializationFormat ?? "c";

        public override TimeSpan Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            var value = reader.GetString();
            if (value == null)
            {
#pragma warning disable CA2208 // Instantiate argument exceptions correctly
                throw new ArgumentNullException(nameof(value));
#pragma warning restore CA2208 // Instantiate argument exceptions correctly
            }

            return TimeSpan.Parse(value);
        }

        public override void Write(Utf8JsonWriter writer, TimeSpan value, JsonSerializerOptions options)
            => writer.WriteStringValue(value.ToString(serializationFormat));
    }
#endif
}
