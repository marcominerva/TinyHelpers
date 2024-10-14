# TimeOnlyTypeHandler Class

A custom Dapper type handler for the `TimeOnly` struct, used to map database time values to `TimeOnly` in .NET 6.0 or greater.

### Usage

This class provides functionality to parse database time values into `TimeOnly` and to convert `TimeOnly` values to a format suitable for database storage.

### `Parse(object value)`

- Parses a database value to a `TimeOnly` object.

#### Parameters
- **`value`** (`object`):  
   Takes a database value expected to be a time-based type and converts it to a `TimeOnly` representation.

#### Returns
- **TimeOnly**:  
   Returns value converted to a `TimeOnly` Structure.

### `SetValue(IDbDataParameter parameter, TimeOnly value)`

- Sets the value of the database parameter to a `TimeOnly` converted to a `TimeSpan`.Configures the database parameter's type as `DbType.Time` to indicate that the value represents a time.

#### Parameters
- **`parameter`** (`IDbDataParameter `):  
  The database parameter.
  
- **`value`** (`IEnumerable<string> `):  
  The TimeOnly value to be set

#### Returns
- **`void`**:  

### Configure Method
- Configures Dapper to use the `TimeOnlyTypeHandler` for handling `TimeOnly` values.
- Should be called during application startup to ensure that Dapper is correctly configured to map `TimeOnly` values to and from the database.
