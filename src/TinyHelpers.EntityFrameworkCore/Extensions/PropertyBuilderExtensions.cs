using System.Text.Json;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TinyHelpers.EntityFrameworkCore.Comparers;
using TinyHelpers.EntityFrameworkCore.Converters;

namespace TinyHelpers.EntityFrameworkCore.Extensions;

public static class PropertyBuilderExtensions
{
    public static PropertyBuilder<DateOnly> HasDateOnlyConversion<DateOnly>(this PropertyBuilder<DateOnly> propertyBuilder)
        => propertyBuilder.HasConversion<DateOnlyConverter, DateOnlyComparer>();

    public static PropertyBuilder<TimeOnly> HasTimeOnlyConversion<TimeOnly>(this PropertyBuilder<TimeOnly> propertyBuilder)
    => propertyBuilder.HasConversion<TimeOnlyConverter, TimeOnlyComparer>();

    public static PropertyBuilder<T?> HasJsonConversion<T>(this PropertyBuilder<T?> propertyBuilder, JsonSerializerOptions? jsonSerializerOptions = null)
    {
        var converter = new JsonStringConverter<T>(jsonSerializerOptions);
        var comparer = new JsonStringComparer<T>(jsonSerializerOptions);

        propertyBuilder.HasConversion(converter, comparer);

        return propertyBuilder;
    }

    public static PropertyBuilder<IEnumerable<string>> HasArrayConversion(this PropertyBuilder<IEnumerable<string>> propertyBuilder, string separator = ";")
    {
        var converter = new StringArrayConverter(separator);
        var comparer = new StringArrayComparer();

        propertyBuilder.HasConversion(converter, comparer);
        return propertyBuilder;
    }
}
