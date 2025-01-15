using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TinyHelpers.EntityFrameworkCore.Comparers;
using TinyHelpers.EntityFrameworkCore.Converters;

namespace TinyHelpers.EntityFrameworkCore.Extensions;

public static class PropertyBuilderExtensions
{
    public static PropertyBuilder<T?> HasJsonConversion<T>(this PropertyBuilder<T?> propertyBuilder, JsonSerializerOptions? jsonSerializerOptions = null, bool useUtcDate = false, bool serializeEnumAsString = false)
    {
        jsonSerializerOptions ??= new(JsonOptions.Default);

        if (useUtcDate)
        {
            jsonSerializerOptions.Converters.Add(new Json.Serialization.UtcDateTimeConverter());
        }

        if (serializeEnumAsString)
        {
            jsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
        }

        var converter = new JsonStringConverter<T>(jsonSerializerOptions);
        var comparer = new JsonStringComparer<T>(jsonSerializerOptions);

        propertyBuilder.HasConversion(converter, comparer);

        return propertyBuilder;
    }

    public static PropertyBuilder<IEnumerable<string>> HasArrayConversion(this PropertyBuilder<IEnumerable<string>> propertyBuilder, string separator = ";")
    {
        var converter = new StringArrayConverter(separator);
        var comparer = new StringArrayComparer();

        propertyBuilder.HasConversion(converter!, comparer);
        return propertyBuilder;
    }

    public static PropertyBuilder<string[]> HasArrayConversion(this PropertyBuilder<string[]> propertyBuilder, string separator = ";")
    {
        var converter = new StringArrayConverter(separator);
        var comparer = new StringArrayComparer();

        propertyBuilder.HasConversion(converter, comparer);
        return propertyBuilder;
    }

    public static PropertyBuilder IsVector(this PropertyBuilder propertyBuilder, int size = 1536)
        => propertyBuilder.HasColumnType($"vector({size})");

    public static PropertyBuilder<T> IsVector<T>(this PropertyBuilder<T> propertyBuilder, int size = 1536)
        => propertyBuilder.HasColumnType($"vector({size})");
}
