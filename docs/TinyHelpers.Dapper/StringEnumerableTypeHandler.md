# StringEnumerableTypeHandler

`StringEnumerableTypeHandler` is a custom type handler used with Dapper to map a string field in a database to an `IEnumerable<string>` in the application, and vice versa.

### Usage

This handler allows you to split a delimited string (e.g., `"apple;banana;cherry"`) from the database into a list or array of strings, and join a list or array of strings into a delimited string when saving to the database.

```csharp
StringEnumerableTypeHandler.Configure(";");
```


### `Parse(object value)`

The `Parse` method converts a database value into an `IEnumerable<string>`, splitting a string using the specified separator.

#### Parameters
- **`value`** (`object`):  
  The database value to be parsed. It is expected to be a string containing multiple values separated by the specified separator.

#### Returns
- **`IEnumerable<string>`**:  
  A collection of strings obtained by splitting the input value based on the separator. Empty entries are removed from the result.

#### Example

```csharp
var result = handler.Parse("apple;banana;cherry");
// result: ["apple", "banana", "cherry"]
```

### `SetValue(IDbDataParameter parameter, IEnumerable<string>? value)`

 The `SetValue` method converts an `IEnumerable<string>` into a single delimited string, suitable for saving to the database, and assigns it to the given database parameter.

#### Parameters
- **`parameter`** (`IDbDataParameter `):  
  The database parameter.
  
- **`value`** (`IEnumerable<string> `):  
  The enumerable collection of values

#### Returns
- **`void`**:  
  
  
#### Example

```csharp
var values = new List<string> { "apple", "banana", "cherry" };
handler.SetValue(myParameter, values);
// myParameter.Value: "apple;banana;cherry"
```
