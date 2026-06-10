using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace TinyHelpers.EntityFrameworkCore.Converters;

public class StringEmptyToNullTrimConverter() : ValueConverter<string?, string?>(
            value => string.IsNullOrWhiteSpace(value) ? null : value.Trim(),
            value => value);