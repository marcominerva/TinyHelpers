using System.Text.Json;
using TinyHelpers.Json.Serialization;
using TinyHelpers.Sample;

// Use converters to customize JSON serialization.
var jsonSerializerOptions = new JsonSerializerOptions(JsonSerializerDefaults.Web);
jsonSerializerOptions.Converters.Add(new StringTrimmingConverter());

var person = new Person("Donald ", "Duck ", " Duckburg  ");
var json = JsonSerializer.Serialize(person);

Console.Write(json);

var result = JsonSerializer.Deserialize<Person>(json, jsonSerializerOptions);
Console.WriteLine(result);

Console.ReadLine();

public record class Person(string FirstName, string LastName, string City);
