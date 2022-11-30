using System.Text.Json;
using TinyHelpers.Extensions;
using TinyHelpers.Json.Serialization;

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
jsonSerializerOptions.Converters.Add(new StringTrimmingConverter());

var person = new Person("Donald ", "Duck ", " Duckburg  ");
var json = JsonSerializer.Serialize(person);

Console.Write(json);

var result = JsonSerializer.Deserialize<Person>(json, jsonSerializerOptions);
Console.WriteLine(result);

// WhereIf
var list = new List<int> { 1, 2, 3 };
list = list.AsQueryable().WhereIf(true, i => i == 2).ToList();
Console.WriteLine(list);

Console.ReadLine();

public record class Person(string FirstName, string LastName, string City);