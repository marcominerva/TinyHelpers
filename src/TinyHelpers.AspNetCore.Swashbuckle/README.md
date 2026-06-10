# Tiny Helpers for Swashbuckle ASP.NET Core

[![Lint Code Base](https://github.com/marcominerva/TinyHelpers/actions/workflows/linter.yml/badge.svg)](https://github.com/marcominerva/TinyHelpers/actions/workflows/linter.yml)
[![CodeQL](https://github.com/marcominerva/TinyHelpers/actions/workflows/github-code-scanning/codeql/badge.svg)](https://github.com/marcominerva/TinyHelpers/actions/workflows/github-code-scanning/codeql)
[![NuGet](https://img.shields.io/nuget/v/TinyHelpers.AspNetCore.Swashbuckle.svg?style=flat-square)](https://www.nuget.org/packages/TinyHelpers.AspNetCore.Swashbuckle)
[![NuGet](https://img.shields.io/nuget/dt/TinyHelpers.AspNetCore.Swashbuckle)](https://www.nuget.org/packages/TinyHelpers.AspNetCore.Swashbuckle)
[![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg)](https://github.com/marcominerva/TinyHelpers/blob/master/LICENSE)

TinyHelpers.AspNetCore.Swashbuckle is a small collection of practical helpers for Swashbuckle ASP.NET Core applications.
It keeps common Swagger configuration in one place so applications can reuse the same OpenAPI conventions without
duplicating setup code across startup files.

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
| `AddAcceptLanguageHeader()` | Adds the `Accept-Language` header to documented operations when the app has supported cultures. | When your API uses request localization and you want consumers to discover the supported culture values. |
| `AddDefaultProblemDetailsResponse()` | Adds a default `application/problem+json` response to operations. | When you want a consistent error contract in Swagger UI and generated documents. |
| `AddTimeSpanTypeMapping(bool useCurrentTimeAsExample = false)` | Maps `TimeSpan` to a string schema and optionally adds a readable example. | When your API exposes `TimeSpan` values and you want a clearer schema. |
| `AddTimeSpanTypeMapping(string? example)` | Maps `TimeSpan` to a string schema using a custom example value. | When you want a precise sample that matches your API format. |
| `AddSwaggerOperationParameters(Action<OpenApiOperationOptions> setupAction)` | Registers reusable OpenAPI parameters in the dependency injection container. | When you want to define shared parameters once and reuse them in Swagger generation. |
| `AddOperationParameters()` | Adds the parameters configured through `OpenApiOperationOptions`. | When you want the shared parameters to appear in the generated Swagger operations. |

### Example

```csharp
using Microsoft.OpenApi;
using TinyHelpers.AspNetCore.Swagger;

builder.Services.AddSwaggerGen(options =>
{
    options.AddAcceptLanguageHeader();
    options.AddDefaultProblemDetailsResponse();
    options.AddTimeSpanTypeMapping(useCurrentTimeAsExample: true);
    options.AddOperationParameters();
});

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
```

## Schema helpers

### `OpenApiSchemaHelper`

This helper provides ready-to-use schema fragments that can be reused when building custom Swagger filters or other
OpenAPI customizations.

| Method | What it does | When to use it |
| --- | --- | --- |
| `CreateStringSchema(string? defaultValue = null)` | Creates a string schema with an optional default value. | When you want a simple reusable string schema. |
| `CreateSchema<TValue>(JsonSchemaType type, string? format = null)` | Creates a schema with an explicit OpenAPI type and format. | When you want a primitive schema and prefer the typed helper overload. |
| `CreateSchema<TValue>(JsonSchemaType type, string? format, TValue? defaultValue = null)` | Creates a schema with a typed default value. | When you want to document a primitive value and its default. |
| `CreateSchema(IEnumerable<string> values, string? defaultValue = null)` | Creates a string schema with an enumeration of allowed values. | When the field is constrained to a fixed set of strings. |
| `CreateSchema<TEnum>(TEnum? defaultValue = null)` | Creates a string schema from an enum type. | When you want the enum names to appear as OpenAPI values. |

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

builder.Services.AddSwaggerGen(options =>
{
    options.AddAcceptLanguageHeader();
    options.AddDefaultProblemDetailsResponse();
    options.AddTimeSpanTypeMapping("00:15:00");
    options.AddOperationParameters();
});

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
