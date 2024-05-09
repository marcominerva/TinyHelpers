# Tiny Helpers for Entity Framework Core

[![Lint Code Base](https://github.com/marcominerva/TinyHelpers/actions/workflows/linter.yml/badge.svg)](https://github.com/marcominerva/TinyHelpers/actions/workflows/linter.yml)
[![CodeQL](https://github.com/marcominerva/TinyHelpers/actions/workflows/codeql.yml/badge.svg)](https://github.com/marcominerva/TinyHelpers/actions/workflows/codeql.yml)
[![NuGet](https://img.shields.io/nuget/v/TinyHelpers.EntityFrameworkCore.svg?style=flat-square)](https://www.nuget.org/packages/TinyHelpers.EntityFrameworkCore)
[![Nuget](https://img.shields.io/nuget/dt/TinyHelpers.EntityFrameworkCore)](https://www.nuget.org/packages/TinyHelpers.EntityFrameworkCore)
[![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg)](https://github.com/marcominerva/TinyHelpers/blob/master/LICENSE)

A collection of helper methods and classes for Entity Framework Core that I use every day. I have packed them in a single library to avoid code duplication.

### Installation

The library is available on [NuGet](https://www.nuget.org/packages/TinyHelpers.EntityFrameworkCore). Just search for *TinyHelpers.EntityFrameworkCore* in the **Package Manager GUI** or run the following command in the **.NET CLI**:

```shell
dotnet add package TinyHelpers.EntityFrameworkCore
```

### Usage

#### Converters

The library provides some [Value Converters](https://docs.microsoft.com/ef/core/modeling/value-conversions) to handle data types that are not natively supported by Entity Framework Core. They can be explicitly used calling the [HasConversion](https://docs.microsoft.com/dotnet/api/microsoft.entityframeworkcore.metadata.builders.propertybuilder.hasconversion) method, or automatically via some extension methods: 

```csharp
// using TinyHelpers.EntityFrameworkCore.Extensions;
protected override void OnModelCreating(ModelBuilder modelBuilder)
{
    modelBuilder.Entity<Post>(builder =>
    {
        builder.Property(x => x.Title).HasMaxLength(80).IsRequired();
        builder.Property(x => x.Content).IsRequired();

        // Date is a DateOnly property (.NET 6 or 7).
        //builder.Property(x => x.Date).HasDateOnlyConversion();

        // Time is a TimeOnly property (.NET 6 or 7).
        //builder.Property(x => x.Time).HasTimeOnlyConversion();

        /* JSON SUPPORT */

        // For .NET 6:
        // Reviews is a complex type, this Converter will automatically JSON-de/serialize it
        // in a string column.
        // builder.Property(x => x.Reviews).HasJsonConversion();

        // For .NET 7 or higher:
        builder.OwnsMany(x => x.Reviews).ToJson();

        /* COLLECTION OF PRIMITIVE TYPES */

        // For .NET 6 and 7:
        //builder.Property(x => x.Authors).HasArrayConversion();

        // For .NET 8
        // The support for collection of primitive types is built-in.
    });
}
```

#### Query Filters

The library provides an extension method that allows to apply a global [Query Filter](https://docs.microsoft.com/ef/core/querying/filters) to entities:

```csharp
public abstract class DeletableEntity
{
    public bool IsDeleted { get; set; }
}

public class Person : DeletableEntity { }

public class City : DeletableEntity { }

public class PhoneNumber : DeletableEntity { }

// using TinyHelpers.EntityFrameworkCore.Extensions;
protected override void OnModelCreating(ModelBuilder modelBuilder)
{
    // Apply a filter to all the entities that inherit (directly or indirectly) from DeletableEntity.
    modelBuilder.ApplyQueryFilter<DeletableEntity>(e => !e.IsDeleted);
}
```

### Contributing

The project is constantly evolving. Contributions are welcome. Feel free to file issues and pull requests on the repo and we'll address them as we can. 

> **Warning**
Remember to work on the **develop** branch, don't use the **master** branch directly. Create Pull Requests targeting **develop**.
