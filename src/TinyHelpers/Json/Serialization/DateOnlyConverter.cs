using System;
using System.Globalization;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace TinyHelpers.Json.Serialization
{
    public class DateOnlyConverter : JsonConverter<DateTime>
    {
        private readonly string serializationFormat;

        public DateOnlyConverter() : this(null)
        {
        }

        public DateOnlyConverter(string? serializationFormat)
            => this.serializationFormat = serializationFormat ?? "yyyy-MM-dd";

        public override DateTime Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
            => DateTimeOffset.Parse(reader.GetString(), CultureInfo.InvariantCulture, DateTimeStyles.AssumeLocal).Date;

        public override void Write(Utf8JsonWriter writer, DateTime value, JsonSerializerOptions options)
            => writer.WriteStringValue(value.ToString(serializationFormat));
    }
}
