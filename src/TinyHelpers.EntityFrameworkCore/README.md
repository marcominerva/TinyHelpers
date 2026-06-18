# Tiny Helpers for Entity Framework Core

[![Lint Code Base](https://github.com/marcominerva/TinyHelpers/actions/workflows/linter.yml/badge.svg)](https://github.com/marcominerva/TinyHelpers/actions/workflows/linter.yml)
[![CodeQL](https://github.com/marcominerva/TinyHelpers/actions/workflows/github-code-scanning/codeql/badge.svg)](https://github.com/marcominerva/TinyHelpers/actions/workflows/github-code-scanning/codeql)
[![NuGet](https://img.shields.io/nuget/v/TinyHelpers.EntityFrameworkCore.svg?style=flat-square)](https://www.nuget.org/packages/TinyHelpers.EntityFrameworkCore)
[![NuGet](https://img.shields.io/nuget/dt/TinyHelpers.EntityFrameworkCore)](https://www.nuget.org/packages/TinyHelpers.EntityFrameworkCore)
[![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg)](https://github.com/marcominerva/TinyHelpers/blob/master/LICENSE)

TinyHelpers.EntityFrameworkCore is a small collection of practical helpers for Entity Framework Core applications: value converters, value comparers, global query filters, transaction helpers, and vector-column mapping.

The package is designed to reduce repetitive configuration and keep the persistence intent of your EF Core model close to the entity type or model configuration that uses it.

## Compatibility

The package targets:

- .NET 8
- .NET 9
- .NET 10

Some features are framework-specific:

- Named query filters are available only on .NET 10+
- The package focuses on EF Core model configuration, conversions, and transaction helpers

## Installation

Install the package from NuGet:

```shell
dotnet add package TinyHelpers.EntityFrameworkCore
```

Or search for `TinyHelpers.EntityFrameworkCore` in the Visual Studio Package Manager.

## Contents

- [Converters and comparers](#converters-and-comparers)
- [Property builder helpers](#property-builder-helpers)
- [Query filters](#query-filters)
- [Transaction helpers](#transaction-helpers)
- [Vector columns](#vector-columns)
- [Quick examples](#quick-examples)
- [Contribute](#contribute)

## Converters and comparers

This area contains helpers for storing common CLR shapes in a single database column while keeping EF Core change tracking aligned with the persisted representation.

### `JsonStringConverter<T>`

Converts a CLR object graph to and from a JSON string for storage in a text column.

Use it when a value object or small object graph belongs to the entity and should live in one text column without manually handling serialization in every entity configuration.

### `JsonStringComparer<T>`

Compares values by their JSON representation.

Use it together with `JsonStringConverter<T>` so EF Core detects changes based on serialized content instead of object reference identity, avoiding redundant updates when two values serialize to the same payload.

### `StringArrayConverter`

Converts a sequence of strings to a single delimiter-separated value.

Use it when a small string collection should stay on the entity row and does not need independent relational querying.

### `StringArrayComparer`

Compares string sequences by content and order.

Use it together with `StringArrayConverter` so EF Core treats two collections as equal when they contain the same values in the same order, even when the collection instances are different.

### `StringEmptyToNullConverter`

Normalizes blank strings to `null` before persistence.

Use it when an empty or whitespace-only value should be treated as the absence of data.

### `StringEmptyToNullTrimConverter`

Normalizes blank strings to `null` and trims meaningful values before persistence.

Use it when user input should not preserve incidental leading or trailing whitespace.

## Property builder helpers

### `PropertyBuilderExtensions`

These helpers keep conversion and comparison pieces together so model configuration can state persistence intent once while EF Core still receives the metadata it needs for materialization and change tracking.

| Method | What it does | When to use it |
| --- | --- | --- |
| `HasJsonConversion<T>(this PropertyBuilder<T?>, JsonSerializerOptions?, bool useUtcDate, bool serializeEnumAsString)` | Stores a property as JSON and wires up matching serialized-value change tracking. | When a value object or small object graph should be persisted in a single text column. |
| `HasArrayConversion(this PropertyBuilder<IEnumerable<string>>)` | Stores a string sequence as a single delimited column and tracks sequence content. | When the property is exposed as `IEnumerable<string>` and does not need a separate relational table. |
| `HasArrayConversion(this PropertyBuilder<string[]>)` | Stores a string array as a single delimited column and tracks sequence content. | When the property is exposed as `string[]` and does not need a separate relational table. |
| `IsVector(this PropertyBuilder, int size = 1536)` | Maps the property to a vector column type with the specified dimension. | When the database supports vector search or embeddings. |
| `IsVector<T>(this PropertyBuilder<T>, int size = 1536)` | Strongly typed version of `IsVector`. | When the property is strongly typed and you want fluent chaining. |

### Example

```csharp
using Microsoft.EntityFrameworkCore;
using TinyHelpers.EntityFrameworkCore.Extensions;

protected override void OnModelCreating(ModelBuilder modelBuilder)
{
    modelBuilder.Entity<Post>(builder =>
    {
        builder.Property(x => x.Metadata).HasJsonConversion();
        builder.Property(x => x.Tags).HasArrayConversion(",");
        builder.Property(x => x.Embedding).IsVector(1536);
    });
}
```

## Query filters

### `ModelBuilderExtensions`

These helpers centralize model-wide conventions such as soft delete, tenant isolation, or visibility rules so each entity type does not need duplicate configuration.

| Method | What it does | When to use it |
| --- | --- | --- |
| `ApplyQueryFilter<TEntity>(Expression<Func<TEntity, bool>>)` | Applies the same filter to all mapped entity types assignable to `TEntity`. | When several entities share a common base type or interface. |
| `ApplyQueryFilter<TType>(string propertyName, TType value)` | Applies a filter to all mapped entity types that expose a property with the given name and type. | When entities do not share a type but expose the same shadow or CLR property. |
| `ApplyQueryFilter<TEntity>(string filterName, Expression<Func<TEntity, bool>>)` | .NET 10+ named filter overload. | When you want to selectively disable one filter later, such as soft delete without disabling tenant isolation. |
| `ApplyQueryFilter<TType>(string filterName, string propertyName, TType value)` | .NET 10+ named filter overload for a property match. | When a shared property drives a filter that may need to be disabled independently. |
| `GetEntityTypes<TType>()` | Returns the mapped CLR entity types assignable to `TType`. | When you need the model types behind a base type or interface. |
| `GetEntityTypes(Type baseType)` | Returns the mapped CLR entity types assignable to a runtime type. | When the target type is only known at runtime. |

### Example

```csharp
using Microsoft.EntityFrameworkCore;
using TinyHelpers.EntityFrameworkCore.Extensions;

public abstract class DeletableEntity
{
    public bool IsDeleted { get; set; }
}

public interface ISoftDeletable
{
    bool IsDeleted { get; set; }
}

protected override void OnModelCreating(ModelBuilder modelBuilder)
{
    modelBuilder.ApplyQueryFilter<DeletableEntity>(e => !e.IsDeleted);
    modelBuilder.ApplyQueryFilter<ISoftDeletable>(e => !e.IsDeleted);
}
```

### .NET 10 named filters

Starting with .NET 10, named filters let you attach a name to each query filter and disable only the ones you need:

```csharp
modelBuilder.ApplyQueryFilter<ISoftDeletable>("SoftDelete", e => !e.IsDeleted);
modelBuilder.ApplyQueryFilter<ITenantEntity>("TenantFilter", e => e.TenantId == currentTenantId);
```

Later, you can disable just one filter:

```csharp
var items = await context.Set<Order>().IgnoreQueryFilters(["SoftDelete"]).ToListAsync();
```

## Transaction helpers

### `DbContextExtensions`

These helpers wrap EF Core execution strategies and explicit transactions so retry behavior stays consistent and transaction boilerplate stays out of repositories and services.

| Method | What it does | When to use it |
| --- | --- | --- |
| `ExecuteTransactionAsync(Func<CancellationToken, Task>)` | Runs work inside a transaction and commits when the action completes. | When you do not need the transaction object itself. |
| `ExecuteTransactionAsync<TResult>(Func<CancellationToken, Task<TResult>>)` | Same as above, but returns a result. | When the unit of work produces a value. |
| `ExecuteTransactionAsync(Func<IDbContextTransaction, CancellationToken, Task>)` | Runs work inside a transaction and passes the active transaction to the callback. The callback decides whether and when to commit it. | When lower-level APIs need direct transaction access or custom commit timing. |
| `ExecuteTransactionAsync<TResult>(Func<IDbContextTransaction, CancellationToken, Task<TResult>>)` | Same as above, but returns a result. | When you need both transaction access, custom commit timing, and a computed value. |

### Example

```csharp
using TinyHelpers.EntityFrameworkCore.Extensions;

await context.ExecuteTransactionAsync(async cancellationToken =>
{
    context.Add(new Order { Id = 1 });
    await context.SaveChangesAsync(cancellationToken);
});
```

## Vector columns

### `VectorAttribute`

Marks a property or field as a vector column.

Use it when your database provider supports vector types and you want the mapping intent to live directly on the entity member.

### Example

```csharp
using System.ComponentModel.DataAnnotations.Schema;

public sealed class Document
{
    public int Id { get; set; }

    [Vector(1536)]
    public float[] Embedding { get; set; } = [];
}
```

## Quick examples

### JSON-backed property mapping

```csharp
using Microsoft.EntityFrameworkCore;
using TinyHelpers.EntityFrameworkCore.Extensions;

protected override void OnModelCreating(ModelBuilder modelBuilder)
{
    modelBuilder.Entity<Post>(builder =>
    {
        builder.Property(x => x.Metadata).HasJsonConversion();
    });
}
```

### String collection mapping

```csharp
using Microsoft.EntityFrameworkCore;
using TinyHelpers.EntityFrameworkCore.Extensions;

protected override void OnModelCreating(ModelBuilder modelBuilder)
{
    modelBuilder.Entity<Post>(builder =>
    {
        builder.Property(x => x.Tags).HasArrayConversion(",");
    });
}
```

### Global soft-delete filter

```csharp
using Microsoft.EntityFrameworkCore;
using TinyHelpers.EntityFrameworkCore.Extensions;

protected override void OnModelCreating(ModelBuilder modelBuilder)
{
    modelBuilder.ApplyQueryFilter<ISoftDeletable>(e => !e.IsDeleted);
}
```

## Contribute

The project is continuously evolving. Contributions, issues, and pull requests are welcome.

> [!WARNING]
> Work on the **develop** branch, not on **master**. Pull requests should target **develop**.
