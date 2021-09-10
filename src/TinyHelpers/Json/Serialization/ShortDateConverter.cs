using System;
using System.Globalization;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace TinyHelpers.Json.Serialization
{
    public class ShortDateConverter : JsonConverter<DateTime>
    {
        private readonly string serializationFormat;

        public ShortDateConverter() : this(null)
        {
        }

        public ShortDateConverter(string? serializationFormat)
            => this.serializationFormat = serializationFormat ?? "yyyy-MM-dd";

        public override DateTime Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            var value = reader.GetString();
            if (value == null)
            {
#pragma warning disable CA2208 // Instantiate argument exceptions correctly
                throw new ArgumentNullException(nameof(value));
#pragma warning restore CA2208 // Instantiate argument exceptions correctly
            }

            return DateTimeOffset.Parse(value, CultureInfo.InvariantCulture, DateTimeStyles.AssumeLocal).Date;
        }

        public override void Write(Utf8JsonWriter writer, DateTime value, JsonSerializerOptions options)
            => writer.WriteStringValue(value.ToString(serializationFormat));
    }
}
