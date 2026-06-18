# Tiny Helpers for Swashbuckle ASP.NET Core

[![Lint Code Base](https://github.com/marcominerva/TinyHelpers/actions/workflows/linter.yml/badge.svg)](https://github.com/marcominerva/TinyHelpers/actions/workflows/linter.yml)
[![CodeQL](https://github.com/marcominerva/TinyHelpers/actions/workflows/github-code-scanning/codeql/badge.svg)](https://github.com/marcominerva/TinyHelpers/actions/workflows/github-code-scanning/codeql)
[![NuGet](https://img.shields.io/nuget/v/TinyHelpers.AspNetCore.Swashbuckle.svg?style=flat-square)](https://www.nuget.org/packages/TinyHelpers.AspNetCore.Swashbuckle)
[![NuGet](https://img.shields.io/nuget/dt/TinyHelpers.AspNetCore.Swashbuckle)](https://www.nuget.org/packages/TinyHelpers.AspNetCore.Swashbuckle)
[![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg)](https://github.com/marcominerva/TinyHelpers/blob/master/LICENSE)

TinyHelpers.AspNetCore.Swashbuckle is a small collection of practical helpers for Swashbuckle ASP.NET Core applications.
It keeps common Swagger configuration in one place so applications can reuse the same OpenAPI conventions without
duplicating setup code across startup files, endpoint metadata, or custom filters.

## Compatibility

The package targets:

- .NET 8
- .NET 9
- .NET 10

It is designed to be used with [Swashbuckle.AspNetCore](https://github.com/domaindrivendev/Swashbuckle.AspNetCore)
and `AddSwaggerGen(...)`.

## Installation

Install the package from NuGet:

```shell
dotnet add package TinyHelpers.AspNetCore.Swashbuckle
```

Or search for `TinyHelpers.AspNetCore.Swashbuckle` in the Visual Studio Package Manager.

## Contents

- [Swagger and OpenAPI helpers](#swagger-and-openapi-helpers)
- [Schema helpers](#schema-helpers)
- [Quick examples](#quick-examples)

## Swagger and OpenAPI helpers

### `SwaggerExtensions`

| Method | What it does | When to use it |
| --- | --- | --- |
| `AddAcceptLanguageHeader()` | Adds the `Accept-Language` header to documented operations when the app has supported cultures. | When your API uses request localization and consumers need to discover supported culture values. |
| `AddDefaultProblemDetailsResponse()` | Adds a default `application/problem+json` response to generated operations. | When you want a consistent error contract in Swagger UI and generated documents. |
| `AddTimeSpanTypeMapping(bool useCurrentTimeAsExample = false)` | Maps `TimeSpan` to a string schema and optionally adds a readable example. | When your API exposes `TimeSpan` values and you want the schema to show the wire format clearly. |
| `AddTimeSpanTypeMapping(string? example)` | Maps `TimeSpan` to a string schema using a custom example value. | When you want a precise sample that matches your API format. |
| `AddSwaggerOperationParameters(Action<OpenApiOperationOptions> setupAction)` | Registers reusable Swagger operation parameters in the dependency injection container. | When cross-cutting headers or query values must be declared once and reused during Swagger generation. |
| `AddOperationParameters()` | Adds the operation filter that copies registered shared parameters into generated Swagger operations. | When you want the generated contract to include the parameters registered with `AddSwaggerOperationParameters()`. |

### Example

```csharp
using Microsoft.OpenApi;
using TinyHelpers.AspNetCore.Swagger;

builder.Services.AddSwaggerOperationParameters(parameters =>
{
    parameters.Parameters.Add(new OpenApiParameter
    {
        Name = "X-Correlation-Id",
        In = ParameterLocation.Header,
        Required = false,
        Description = "Identifier used to correlate requests"
    });
});

builder.Services.AddSwaggerGen(options =>
{
    options.AddAcceptLanguageHeader();
    options.AddDefaultProblemDetailsResponse();
    options.AddTimeSpanTypeMapping(useCurrentTimeAsExample: true);
    options.AddOperationParameters();
});
```

Register shared parameters with `AddSwaggerOperationParameters()` during service registration, then enable
`AddOperationParameters()` in `AddSwaggerGen(...)`. The options object stores the shared parameter definitions, and the
operation filter copies them into generated operations. This prevents duplicated endpoint metadata while preserving a
complete contract for Swagger UI and generated clients.

## Schema helpers

### `OpenApiSchemaHelper`

This helper provides ready-to-use schema fragments that can be reused when building custom Swagger filters or other
OpenAPI customizations. It keeps default values, formats, and enum choices consistent across generated documents.

| Method | What it does | When to use it |
| --- | --- | --- |
| `CreateStringSchema(string? defaultValue = null)` | Creates a reusable string schema with an optional default value. | When you want text-based contract metadata without rebuilding the same schema each time. |
| `CreateSchema<TValue>(JsonSchemaType type, string? format = null)` | Creates a primitive schema with explicit OpenAPI type and format metadata. | When a filter needs to describe a primitive OpenAPI shape consistently. |
| `CreateSchema<TValue>(JsonSchemaType type, string? format, TValue? defaultValue = null)` | Creates a primitive schema and includes the default value that clients should display or assume. | When you want to document both the value shape and its fallback. |
| `CreateSchema(IEnumerable<string> values, string? defaultValue = null)` | Creates a string schema with an enumeration of externally defined allowed values. | When the field is constrained to a fixed set of strings that is not represented by a CLR enum. |
| `CreateSchema<TEnum>(TEnum? defaultValue = null)` | Creates a string schema from an enum type so every declared value is documented. | When you want CLR enum names to appear as OpenAPI values. |

### Example

```csharp
using TinyHelpers.AspNetCore.Swagger;

var cultureSchema = OpenApiSchemaHelper.CreateSchema(["it-IT", "en-US"], "it-IT");
var durationSchema = OpenApiSchemaHelper.CreateStringSchema("00:30:00");
```

### Example with an enum

```csharp
public enum ExportFormat
{
    Csv,
    Json,
    Xml
}

var schema = OpenApiSchemaHelper.CreateSchema<ExportFormat>(ExportFormat.Json);
```

## Quick examples

### Minimal API with Swagger helpers

```csharp
using Microsoft.OpenApi;
using Swashbuckle.AspNetCore.SwaggerGen;
using TinyHelpers.AspNetCore.Swagger;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSwaggerOperationParameters(parameters =>
{
    parameters.Parameters.Add(new OpenApiParameter
    {
        Name = "X-Request-Id",
        In = ParameterLocation.Header,
        Required = false,
        Description = "Optional request correlation identifier"
    });
});

builder.Services.AddSwaggerGen(options =>
{
    options.AddAcceptLanguageHeader();
    options.AddDefaultProblemDetailsResponse();
    options.AddTimeSpanTypeMapping("00:15:00");
    options.AddOperationParameters();
});

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.Run();
```

### Building a reusable schema

```csharp
using Microsoft.OpenApi;
using TinyHelpers.AspNetCore.Swagger;

var schema = OpenApiSchemaHelper.CreateSchema<string>(JsonSchemaType.String, "uuid");
```

## Contribute

The project is continuously evolving. Contributions, issues, and pull requests are welcome.

> [!WARNING]
> Work on the **develop** branch, not on **master**. Pull requests should target **develop**.
