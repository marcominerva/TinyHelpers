# StringArrayTypeHandler Class

A custom Dapper type handler for mapping arrays of strings to and from the database. 

### Usage

This handler allows the conversion of a delimited string in the database to a string array in the application, and vice versa. A custom separator can be defined to split or join the string elements.

```csharp
StringArrayTypeHandler.Configure(";");
```


### `Parse(object value)`

Converts a database value, expected to be a delimited string, into a `string[]`.

#### Parameters
- **`value`** (`object`):  
The database value to be parsed. It is expected to be a string containing multiple values separated by the specified separator.

#### Returns
- **`string[]`**:  
A array of strings obtained by splitting the input value based on the separator. Empty entries are removed from the result.

### `SetValue(IDbDataParameter parameter, string[]? value)`

The `SetValue` method converts an `string[]` array into a single delimited string, suitable for saving to the database, and assigns it to the given database parameter.

#### Parameters
- **`parameter`** (`IDbDataParameter `):  
The database parameter.
  
- **`value`** (`string[]? value `):  
The string array of values

#### Returns
- **`void`**:  

### `Configure(string separator = ";")`
Configures Dapper to use the `StringArrayTypeHandler` for handling string array mappings. Allows specifying a custom separator (default is `";"`).
Should be called during application startup to ensure that Dapper is configured to correctly map arrays of strings.
