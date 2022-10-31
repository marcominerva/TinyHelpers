using System.Text.Json;
using System.Text.Json.Serialization;
using BenchmarkDotNet.Attributes;

namespace TinyHelpers.Sample;

[MemoryDiagnoser(false)]
public class Benchmarks
{
    private readonly JsonSerializerOptions? defaultOptions;
    private readonly JsonSerializerOptions? spanOptions;

    [GlobalSetup]
    public void Setup()
    {
        var defaultOptions = new JsonSerializerOptions(JsonSerializerDefaults.Web);
        defaultOptions.Converters.Add(new DefaultStringTrimmingConverter());

        var spanOptions = new JsonSerializerOptions(JsonSerializerDefaults.Web);
        spanOptions.Converters.Add(new SpanStringTrimmingConverter());
    }

    [Benchmark]
    public void DefaultJsonStringTrimming()
    {
        var person = new Person("Donald  ", "Duck ", " Duckburg  ");
        var json = JsonSerializer.Serialize(person, defaultOptions);
        _ = JsonSerializer.Deserialize<Person>(json, defaultOptions);
    }

    [Benchmark]
    public void SpanJsonStringTrimming()
    {
        var person = new Person("Donald  ", "Duck ", " Duckburg  ");
        var json = JsonSerializer.Serialize(person, spanOptions);
        _ = JsonSerializer.Deserialize<Person>(json, spanOptions);
    }
}

public class DefaultStringTrimmingConverter : JsonConverter<string>
{
    public override string? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        => reader.GetString()?.Trim();

    public override void Write(Utf8JsonWriter writer, string value, JsonSerializerOptions options)
        => writer.WriteStringValue(value?.Trim());
}

public class SpanStringTrimmingConverter : JsonConverter<string>
{
    public override string? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        var input = reader.GetString();
        if (input is null)
        {
            return null;
        }

        return input.AsSpan().Trim().ToString();
    }

    public override void Write(Utf8JsonWriter writer, string value, JsonSerializerOptions options)
        => writer.WriteStringValue(value?.Trim());
}