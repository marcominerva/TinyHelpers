# Tiny Helpers for Dapper

[![Lint Code Base](https://github.com/marcominerva/TinyHelpers/actions/workflows/linter.yml/badge.svg)](https://github.com/marcominerva/TinyHelpers/actions/workflows/linter.yml)
[![CodeQL](https://github.com/marcominerva/TinyHelpers/actions/workflows/github-code-scanning/codeql/badge.svg)](https://github.com/marcominerva/TinyHelpers/actions/workflows/github-code-scanning/codeql)
[![NuGet](https://img.shields.io/nuget/v/TinyHelpers.Dapper.svg?style=flat-square)](https://www.nuget.org/packages/TinyHelpers.Dapper)
[![NuGet](https://img.shields.io/nuget/dt/TinyHelpers.Dapper)](https://www.nuget.org/packages/TinyHelpers.Dapper)
[![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg)](https://github.com/marcominerva/TinyHelpers/blob/master/LICENSE)

TinyHelpers.Dapper is a small collection of practical Dapper type handlers for .NET applications.

The package helps reduce repetitive mapping code when your database stores values as JSON, delimited strings, or date/time-specific types. Each handler centralizes the conversion contract so queries and repositories do not need custom parsing or serialization logic at every call site.

## Compatibility

The package targets:

- .NET Standard 2.0
- .NET 8
- .NET 9
- .NET 10

Notes:

- `DateOnlyTypeHandler` and `TimeOnlyTypeHandler` are available only on .NET 6+ targets, so they are not compiled for .NET Standard 2.0.
- `JsonTypeHandler<T>` can be used with any CLR type that Dapper maps to and from JSON columns.

## Installation

Install from [NuGet](https://www.nuget.org/packages/TinyHelpers.Dapper) by searching for `TinyHelpers.Dapper` in Visual Studio Package Manager or using the .NET CLI:

```shell
dotnet add package TinyHelpers.Dapper
```

## Contents

- [Type handlers](#type-handlers)
- [Usage](#usage)
- [Quick examples](#quick-examples)
- [Contribute](#contribute)

## Type handlers

### `JsonTypeHandler<T>`

Reads and writes a CLR type as JSON.

Use it when you want to store a value object or small object graph in a single column and keep serializer configuration centralized instead of repeating it in each query or repository. The optional configuration flags let the handler match the JSON contract expected by the database and consuming clients.

### `StringArrayTypeHandler`

Converts `string[]` values to a single delimited string.

Use it when a database column stores a small set of string values and you want to work with arrays in application code without custom split/join logic. Keep the separator aligned with the stored format so values round-trip reliably.

### `StringEnumerableTypeHandler`

Converts `IEnumerable<string>` values to a single delimited string.

Use it when you want consuming code to stay sequence-oriented while still persisting a simple delimited representation. Keep the separator aligned with the stored format so query results can reconstruct the sequence correctly.

### `DateOnlyTypeHandler`

Maps `DateOnly` values to a database `date` column.

Use it when you want to persist calendar dates without introducing an artificial time component or repeating manual `DateTime` conversions.

### `TimeOnlyTypeHandler`

Maps `TimeOnly` values to a database `time` column.

Use it when you want to persist a time of day without tying it to a calendar date.

### `TimeSpanTypeHandler`

Maps `TimeSpan` values using ticks.

Use it when you want a stable, culture-independent representation for durations that can be round-tripped across supported target frameworks.

## Usage

Register the handlers once at application startup by calling their `Configure` methods. Dapper type handlers are global, so startup registration keeps conversion behavior consistent for all queries and parameters.

### JSON mapping

```csharp
using System.Text.Json;
using TinyHelpers.Dapper.TypeHandlers;

JsonTypeHandler<Person>.Configure(
    new JsonSerializerOptions(JsonSerializerDefaults.Web),
    useUtcDate: true,
    serializeEnumAsString: true);
```

### String collection mapping

```csharp
using TinyHelpers.Dapper.TypeHandlers;

StringArrayTypeHandler.Configure(",");
StringEnumerableTypeHandler.Configure(",");
```

Use the same separator that is already used by the database column format.

### Date and time mapping

```csharp
using TinyHelpers.Dapper.TypeHandlers;

TimeSpanTypeHandler.Configure();

DateOnlyTypeHandler.Configure();
TimeOnlyTypeHandler.Configure();
```

`DateOnlyTypeHandler` and `TimeOnlyTypeHandler` are not available on .NET Standard 2.0.

## Quick examples

### JSON-backed property mapping

```csharp
using System.Text.Json;
using TinyHelpers.Dapper.TypeHandlers;

JsonTypeHandler<Metadata>.Configure(
    new JsonSerializerOptions(JsonSerializerDefaults.Web),
    useUtcDate: true,
    serializeEnumAsString: true);

var post = await connection.QuerySingleAsync<Post>(
    "select Id, Metadata from Posts where Id = @Id",
    new { Id = 1 });
```

### String collection mapping

```csharp
using TinyHelpers.Dapper.TypeHandlers;

StringArrayTypeHandler.Configure(",");

var post = await connection.QuerySingleAsync<Post>(
    "select Id, Tags from Posts where Id = @Id",
    new { Id = 1 });
```

### Date and time mapping

```csharp
using TinyHelpers.Dapper.TypeHandlers;

DateOnlyTypeHandler.Configure();
TimeOnlyTypeHandler.Configure();
TimeSpanTypeHandler.Configure();

var schedule = await connection.QuerySingleAsync<Schedule>(
    "select StartDate, StartTime, Duration from Schedules where Id = @Id",
    new { Id = 1 });
```

## Contribute

The project is continuously evolving. Contributions, issues, and pull requests are welcome.

> [!WARNING]
> Work on the **develop** branch, not on **master**. Pull requests should target **develop**.
