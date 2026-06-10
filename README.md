# Tiny .NET Helpers

[![Lint Code Base](https://github.com/marcominerva/TinyHelpers/actions/workflows/linter.yml/badge.svg)](https://github.com/marcominerva/TinyHelpers/actions/workflows/linter.yml)
[![CodeQL](https://github.com/marcominerva/TinyHelpers/actions/workflows/github-code-scanning/codeql/badge.svg)](https://github.com/marcominerva/TinyHelpers/actions/workflows/github-code-scanning/codeql)
[![NuGet](https://img.shields.io/nuget/v/TinyHelpers.svg?style=flat-square)](https://www.nuget.org/packages/TinyHelpers)
[![NuGet](https://img.shields.io/nuget/dt/TinyHelpers)](https://www.nuget.org/packages/TinyHelpers)
[![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg)](https://github.com/marcominerva/TinyHelpers/blob/master/LICENSE)

TinyHelpers is a small .NET utility library that groups common helpers into a single package. It is designed to reduce repeated boilerplate in application code while keeping each helper focused and easy to discover.

## Compatibility

The package targets:

- .NET Standard 2.0
- .NET 8
- .NET 9
- .NET 10

Some helpers are available only on newer frameworks when the underlying platform adds the required APIs.

## Installation

Install the package from NuGet:

```shell
dotnet add package TinyHelpers
```

Or search for `TinyHelpers` in the Visual Studio Package Manager.

## Contents

- [Collections and sequences](#collections-and-sequences)
- [Strings](#strings)
- [Enums, reflection, and comparisons](#enums-reflection-and-comparisons)
- [Dates, times, and ranges](#dates-times-and-ranges)
- [GUID helpers](#guid-helpers)
- [HTTP helpers](#http-helpers)
- [JSON serialization](#json-serialization)
- [Threading](#threading)
- [Compatibility helpers](#compatibility-helpers)
- [Quick examples](#quick-examples)

## Collections and sequences

### `CollectionExtensions`

| Method | What it does | When to use it |
| --- | --- | --- |
| `EmptyIfNull()` | Returns an empty sequence when the source is `null`. | When you want to avoid null checks before enumeration. |
| `ForEach(Action<T>)` | Executes an action for each item in a sequence. | When you want a simple side-effect loop. |
| `ForEachAsync(Func<T, Task>)` | Executes an asynchronous action for each item. | When each item requires asynchronous work. |
| `SelectAsync(Func<T, Task<TResult>>)` | Projects each item asynchronously. | When the projection itself is asynchronous. |
| `ToListAsync()` | Materializes an asynchronous sequence into a list. | When you need a list from an async source. |
| `Remove(predicate)` | Removes matching items from a collection. | When you want to filter in place. |
| `IsEmpty()` / `IsNotEmpty()` | Checks whether a collection contains items. | When you want readable emptiness checks. |
| `IsNullOrEmpty()` / `IsNotNullOrEmpty()` | Checks for `null` or empty sequences. | When the source may be missing entirely. |
| `HasItems()` | Returns whether a sequence contains at least one item. Returns `false` when the sequence is `null`. | When you only care about presence. |
| `GetCount()` / `GetLongCount()` | Returns the number of elements in a collection, or `0` when the collection is `null`. | When you want a safe count for an optional collection. |
| `WhereIf(condition, predicate)` | Applies a filter only when the condition is true, making it easy to compose multiple conditional filters with fluent syntax instead of chaining separate `if` statements. | When you want to build search queries progressively, especially with Entity Framework Core. |

### Example

```csharp
using TinyHelpers.Extensions;

public IQueryable<Person> SearchPeople(string? nameInitials, string? surnameInitials, string? city)
{
    return context.People
        .WhereIf(!string.IsNullOrWhiteSpace(nameInitials), person => person.FirstName.StartsWith(nameInitials!))
        .WhereIf(!string.IsNullOrWhiteSpace(surnameInitials), person => person.LastName.StartsWith(surnameInitials!))
        .WhereIf(!string.IsNullOrWhiteSpace(city), person => person.City == city);
}

// Example:
// SearchPeople("Ma", "Ro", "Taggia") filters by first name, last name, and city.
// SearchPeople(null, null, "Taggia") filters only by city.
```

## Strings

### `StringExtensions`

| Method | What it does | When to use it |
| --- | --- | --- |
| `EqualsIgnoreCase()` | Compares two strings without case sensitivity. | When casing should not affect equality. |
| `StartsWithIgnoreCase()` | Checks whether a string starts with a value ignoring case. | When matching prefixes in a user-friendly way. |
| `EndsWithIgnoreCase()` | Checks whether a string ends with a value ignoring case. | When matching suffixes without casing concerns. |
| `ContainsIgnoreCase()` | Checks whether a string contains a value ignoring case. | When searching text case-insensitively. |
| `ReplaceIgnoreCase()` | Replaces text without case sensitivity. | When performing forgiving text replacement. |
| `GetValueOrDefault()` | Returns a fallback value when the string is empty. | When you want a safe default value. |
| `HasValue()` | Checks whether a string is not `null`, empty, or whitespace. | When validating user input. |
| `FirstCharToUpper()` | Capitalizes the first character. | When normalizing display text. |

### Example

```csharp
using TinyHelpers.Extensions;

var value = "tiny helpers";
var display = value.FirstCharToUpper();
var contains = value.ContainsIgnoreCase("HELP");
```

## Enums, reflection, and comparisons

### `EnumExtensions`

| Method | What it does | When to use it |
| --- | --- | --- |
| `GetDescription()` | Reads the `DescriptionAttribute` value for an enum member. | When you want a human-readable label. |
| `GetFlags()` | Splits a flags enum into its individual values. | When you need to inspect combined flags. |
| `GetDescriptions()` | Returns descriptions for all enum values. | When building lists or UI options. |

### Other helpers

- `IComparableExtensions.IsBetween()` checks whether a value falls inside a range.
- `AssemblyExtensions.GetAttribute()` reads a custom attribute from an assembly.

### Example

```csharp
using TinyHelpers.Extensions;

var description = ConsoleColor.DarkBlue.GetDescription();
var insideRange = 5.IsBetween(1, 10);
```

## Dates, times, and ranges

### `DateTimeExtensions`

| Method | What it does |
| --- | --- |
| `ToDateOnly()` | Converts a `DateTime` to `DateOnly`. |
| `ToTimeOnly()` | Converts a `DateTime` to `TimeOnly`. |

### `DateTimeOffsetExtensions`

| Method | What it does |
| --- | --- |
| `ToDateOnly()` | Converts a `DateTimeOffset` to `DateOnly`. |
| `ToTimeOnly()` | Converts a `DateTimeOffset` to `TimeOnly`. |

### `DateOnlyExtensions`

| Method | What it does |
| --- | --- |
| `ToDateTimeOffset()` | Converts a `DateOnly` to `DateTimeOffset`. |

### `TimeSpanExtensions`

| Method | What it does |
| --- | --- |
| `ToTimeOnly()` | Converts a `TimeSpan` to `TimeOnly`. |

### `RangeExtensions`

| Method | What it does |
| --- | --- |
| `GetEnumerator()` | Iterates over a `Range` as a sequence of integers. |

### `StopwatchExtensions`

| Method | What it does |
| --- | --- |
| `GetElapsedAndRestart()` | Returns the elapsed time and restarts the stopwatch. |

### Example

```csharp
using TinyHelpers.Extensions;

var createdAt = DateTime.UtcNow;
var dateOnly = createdAt.ToDateOnly();
var timeOnly = createdAt.ToTimeOnly();
```

## GUID helpers

### `GuidExtensions`

| Method | What it does | When to use it |
| --- | --- | --- |
| `IsEmpty()` | Checks whether a GUID is `Guid.Empty`. | When validating identifiers. |
| `IsNotEmpty()` | Checks whether a GUID is not empty. | When you need a positive validation. |
| `HasValue()` | Checks whether a GUID is different from `null` and `Guid.Empty`. | When handling optional identifiers. |
| `GetValueOrCreateNew()` | Returns the existing GUID when it is different from `null` and `Guid.Empty`; otherwise, it creates a new one automatically. On .NET 9+, you can also specify the GUID version to generate. | When you want to assign an identifier lazily. |
| `GetValueOrDefault()` | Returns the current GUID when it is different from `Guid.Empty`; otherwise, it returns a default value. | When null-safe defaults are useful. |

### Example

```csharp
using TinyHelpers.Extensions;

Guid? id = null;
var hasValue = id.HasValue();
var safeId = id.GetValueOrCreateNew();
var defaultId = id.GetValueOrDefault();
```

## HTTP helpers

### `AuthenticatedParameterizedHttpClientHandler`

Adds an access token to outgoing requests through a delegate that can inspect the current `HttpRequestMessage`. If a request returns `401 Unauthorized` and a refresh delegate is configured, the handler can refresh the token and retry the request once. The authorization scheme is configurable and defaults to `Bearer`.

Use this handler when the token depends on the outgoing request or when you need a lightweight refresh flow around a protected API.

### Example

```csharp
var handler = new AuthenticatedParameterizedHttpClientHandler(
    async request => await tokenProvider.GetTokenAsync(request),
    async request => await tokenProvider.RefreshTokenAsync(request),
    innerHandler: new HttpClientHandler());

var client = new HttpClient(handler);
```

### `HeaderInjectorHttpClientHandler`

Adds headers to outgoing requests by invoking a delegate that returns a dictionary of header names and values. Each returned header is applied with `TryAddWithoutValidation`, so the handler is suitable for custom or preformatted header values.

Use this handler when headers such as correlation IDs, tenant identifiers, or custom API metadata need to be injected per request.

### Example

```csharp
var handler = new HeaderInjectorHttpClientHandler(async request =>
    new Dictionary<string, string>
    {
        ["X-Correlation-Id"] = Guid.NewGuid().ToString(),
        ["X-Tenant"] = "contoso"
    }, innerHandler: new HttpClientHandler());

var client = new HttpClient(handler);
```

### `QueryStringInjectorHttpClientHandler`

Adds query string parameters to outgoing requests by merging the values returned by a delegate into the current request URI. Existing query string values are preserved, and new values are appended to the request URL.

Use this handler when the query string depends on the current request context and you want to centralize URL composition.

### Example

```csharp
var handler = new QueryStringInjectorHttpClientHandler(async request =>
    new Dictionary<string, string>
    {
        ["page"] = "1",
        ["pageSize"] = "25"
    }, innerHandler: new HttpClientHandler());

var client = new HttpClient(handler)
{
    BaseAddress = new Uri("https://api.example.com/")
};
```

## JSON serialization

### `ShortDateConverter`

Serializes a `DateTime` using only the date portion.

### `UtcDateTimeConverter`

Serializes and deserializes `DateTime` values in UTC format.

### `TimeSpanTicksConverter`

Serializes a `TimeSpan` as ticks.

### `StringTrimmingConverter`

Trims whitespace from JSON strings during read and write operations.

### `StringEnumMemberConverter`

Converts enum values using `EnumMember` metadata. This type is kept for backward compatibility.

### Example

```csharp
var options = new JsonSerializerOptions
{
    Converters =
    {
        new UtcDateTimeConverter(),
        new ShortDateConverter(),
        new TimeSpanTicksConverter()
    }
};
```

## Threading

### `AsyncLock`

Provides an asynchronous lock that serializes access to a shared resource without blocking threads. It supports an unlimited wait, a timeout in milliseconds, or a `TimeSpan` timeout. The timed overloads return `LockResult`, which tells you whether the lock was acquired.

### Example

```csharp
using TinyHelpers.Threading;

var queue = new Queue<string>(new[] { "Order-1001", "Order-1002", "Order-1003" });
var asyncLock = new AsyncLock();

var readers = new[]
{
    ReadNextAsync("Worker-1", asyncLock, queue),
    ReadNextAsync("Worker-2", asyncLock, queue),
    ReadNextAsync("Worker-3", asyncLock, queue)
};

await Task.WhenAll(readers);

static async Task ReadNextAsync(string workerName, AsyncLock queueLock, Queue<string> queue)
{
    await using var handle = await queueLock.LockAsync();

    if (queue.Count == 0)
    {
        return;
    }

    var item = queue.Dequeue();
    await Task.Delay(Random.Shared.Next(100, 500));

    Console.WriteLine($"{workerName} processed {item}");
}
```

### Timed acquisition

```csharp
using TinyHelpers.Threading;

var (isOwned, queueLock) = await asyncLock.LockAsync(TimeSpan.FromSeconds(2));

if (isOwned && queueLock is not null)
{
    try
    {
        // read or update the shared queue here
    }
    finally
    {
        queueLock.Dispose();
    }
}
```

## Quick examples

### Filtering a sequence conditionally

```csharp
using TinyHelpers.Extensions;

var numbers = new[] { 1, 2, 3, 4, 5 };
var evenNumbers = numbers.WhereIf(true, number => number % 2 == 0);
```

### Reading a readable enum value

```csharp
using TinyHelpers.Extensions;

var label = DayOfWeek.Monday.GetDescription();
```

### Converting dates

```csharp
using TinyHelpers.Extensions;

var today = DateTime.UtcNow.ToDateOnly();
```

### Working with asynchronous locks

```csharp
await using var lease = await asyncLock.LockAsync();
// critical section
```

## Contribute

The project is continuously evolving. Contributions, issues, and pull requests are welcome.

> [!WARNING]
> Work on the **develop** branch, not on **master**. Pull requests should target **develop**.
