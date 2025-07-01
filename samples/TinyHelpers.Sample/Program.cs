using System.Text.Json;
using TinyHelpers.Enums;
using TinyHelpers.Extensions;
using TinyHelpers.Json.Serialization;

// New IsBetween functionality with BoundaryType enum
var number = 5;
Console.WriteLine($"Number: {number}");
Console.WriteLine($"Is between 1 and 10 (Inclusive): {number.IsBetween(1, 10, BoundaryType.Inclusive)}");
Console.WriteLine($"Is between 1 and 10 (Exclusive): {number.IsBetween(1, 10, BoundaryType.Exclusive)}");
Console.WriteLine($"Is between 1 and 10 (LowerInclusive): {number.IsBetween(1, 10, BoundaryType.LowerInclusive)}");
Console.WriteLine($"Is between 1 and 10 (UpperInclusive): {number.IsBetween(1, 10, BoundaryType.UpperInclusive)}");
Console.WriteLine($"Is between 1 and 10 (LowerExclusive): {number.IsBetween(1, 10, BoundaryType.LowerExclusive)}");
Console.WriteLine($"Is between 1 and 10 (UpperExclusive): {number.IsBetween(1, 10, BoundaryType.UpperExclusive)}");

// Test with boundary values
var boundaryNumber = 1;
Console.WriteLine($"\nBoundary Number: {boundaryNumber}");
Console.WriteLine($"Is between 1 and 10 (Inclusive): {boundaryNumber.IsBetween(1, 10, BoundaryType.Inclusive)}");
Console.WriteLine($"Is between 1 and 10 (Exclusive): {boundaryNumber.IsBetween(1, 10, BoundaryType.Exclusive)}");
Console.WriteLine($"Is between 1 and 10 (LowerInclusive): {boundaryNumber.IsBetween(1, 10, BoundaryType.LowerInclusive)}");
Console.WriteLine($"Is between 1 and 10 (UpperInclusive): {boundaryNumber.IsBetween(1, 10, BoundaryType.UpperInclusive)}");

// Distinct By (.NET Standard 2.0)
var people = new List<Person>
{
    new("Marco", "Minerva", "Taggia"),
    new("Andrea", "Bianchi", "Taggia"),
    new("Marco", "Minerva", "Taggia")
};

var distinctPeople = people.DistinctBy(p => new { p.FirstName, p.LastName });
// Extracts Marco Minerva and Andrea Bianchi
Console.WriteLine(distinctPeople);

// Use converters to customize JSON serialization.
var jsonSerializerOptions = new JsonSerializerOptions(JsonSerializerDefaults.Web);
jsonSerializerOptions.Converters.Add(new UtcDateTimeConverter());
jsonSerializerOptions.Converters.Add(new StringTrimmingConverter());

var person = new Person("Donald ", "Duck ", " Duckburg  ") { DateOfBirth = DateTime.Now };
var json = JsonSerializer.Serialize(person, jsonSerializerOptions);

Console.Write(json);

var result = JsonSerializer.Deserialize<Person>(json, jsonSerializerOptions);
Console.WriteLine(result);

// WhereIf
var list = new List<int> { 1, 2, 3 };
list = list.AsQueryable().WhereIf(true, i => i == 2).ToList();
Console.WriteLine(list);

Test(list);

Console.ReadLine();

static void Test(IEnumerable<int>? list)
{
    if (list.HasItems())
    {
        Console.WriteLine(list.Count());
    }
}

public record class Person(string FirstName, string LastName, string City)
{
    public DateTime DateOfBirth { get; set; }
}