using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace TinyHelpers.Json.Serialization
{
    public class TimeSpanConverter : JsonConverter<TimeSpan>
    {
        private readonly string serializationFormat;

        public TimeSpanConverter() : this(null)
        {
        }

        public TimeSpanConverter(string? serializationFormat)
            => this.serializationFormat = serializationFormat ?? @"hh\:mm\:ss";

        public override TimeSpan Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
            => TimeSpan.Parse(reader.GetString());

        public override void Write(Utf8JsonWriter writer, TimeSpan value, JsonSerializerOptions options)
            => writer.WriteStringValue(value.ToString(serializationFormat));
    }
}
