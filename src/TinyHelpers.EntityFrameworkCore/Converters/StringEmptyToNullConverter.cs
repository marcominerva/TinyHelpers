using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace TinyHelpers.EntityFrameworkCore.Converters;

public class StringEmptyToNullConverter() : ValueConverter<string?, string?>(
            value => string.IsNullOrWhiteSpace(value) ? null : value,
            value => value);