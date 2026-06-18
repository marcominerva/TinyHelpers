# Tiny Helpers for ASP.NET Core

[![Lint Code Base](https://github.com/marcominerva/TinyHelpers/actions/workflows/linter.yml/badge.svg)](https://github.com/marcominerva/TinyHelpers/actions/workflows/linter.yml)
[![CodeQL](https://github.com/marcominerva/TinyHelpers/actions/workflows/github-code-scanning/codeql/badge.svg)](https://github.com/marcominerva/TinyHelpers/actions/workflows/github-code-scanning/codeql)
[![NuGet](https://img.shields.io/nuget/v/TinyHelpers.AspNetCore.svg?style=flat-square)](https://www.nuget.org/packages/TinyHelpers.AspNetCore)
[![NuGet](https://img.shields.io/nuget/dt/TinyHelpers.AspNetCore)](https://www.nuget.org/packages/TinyHelpers.AspNetCore)
[![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg)](https://github.com/marcominerva/TinyHelpers/blob/master/LICENSE)

TinyHelpers.AspNetCore is a small collection of practical helpers for ASP.NET Core applications: claims handling, configuration binding, localization, middleware support, validation attributes, route helpers, OpenAPI helpers, and exception handling.

The package is designed to reduce code duplication and keep common behavior centralized in one place.

> [!IMPORTANT]
> **Upgrade from 3.x to 4.x**
> Swashbuckle / Swagger support has been moved to the separate [TinyHelpers.AspNetCore.Swashbuckle](https://github.com/marcominerva/TinyHelpers/tree/master/src/TinyHelpers.AspNetCore.Swashbuckle) package.
> If you were using extensions such as `AddAcceptLanguageHeader()` and `AddDefaultProblemDetailsResponse()` together with `AddSwaggerGen`, you now need to install that package as well.

## Compatibility

The package targets:

- .NET 8
- .NET 9
- .NET 10

OpenAPI features are available when the consuming project targets .NET 9 or .NET 10. Some extensions are framework-specific:

- `EnableEnumSupport()` is available only on .NET 9
- `UseStrictNumericSchemas()` is available only on .NET 10+
- `WithResponseDescription()` and `WithLocationHeader()` are available only on .NET 10+

## Installation

Install the package from NuGet:

```shell
dotnet add package TinyHelpers.AspNetCore
```

Or search for `TinyHelpers.AspNetCore` in the Visual Studio Package Manager.

## Contents

- [Claims handling](#claims-handling)
- [Configuration and options](#configuration-and-options)
- [Localization and pipeline](#localization-and-pipeline)
- [Validation and authorization attributes](#validation-and-authorization-attributes)
- [Route and Minimal API helpers](#route-and-minimal-api-helpers)
- [OpenAPI](#openapi)
- [Exception handling](#exception-handling)
- [Quick examples](#quick-examples)

## Claims handling

This area contains two groups of extensions:

- helpers for `IList<Claim>` that modify the collection
- helpers for `ClaimsPrincipal` that read values without repeating the same lookup logic

### `ClaimsExtensions`

| Method | What it does | When to use it |
| --- | --- | --- |
| `Update(string type, string value)` | Removes the first claim with that type and adds a new one with the specified value. | When you want to replace an identity value without leaving logical duplicates. |
| `Remove(string type)` | Removes the first claim with that type. | When you want to remove a specific value from the list. |

### `ClaimsPrincipalExtensions`

| Method | What it does | When to use it |
| --- | --- | --- |
| `GetClaimValues(string type)` | Returns all claim values as `string?`. | When a claim can have multiple values. |
| `GetClaimValues<T>(string type)` | Returns all values converted to type `T`. | When you need typed conversion of claim values. |
| `GetClaimValue(string type)` | Returns the first matching value as `string?`. | When the claim is single-value. |
| `GetClaimValue<T>(string type)` | Returns the first matching value converted to `T?`. | When you want a typed value with `default` fallback. |
| `HasClaim(string type)` | Checks whether at least one claim with that type exists. | When you need a quick presence check. |

### Example

```csharp
using System.Security.Claims;
using TinyHelpers.AspNetCore.Extensions;

var claims = new List<Claim>
{
    new(ClaimTypes.Name, "Marco"),
    new(ClaimTypes.Role, "Admin")
};

claims.Update(ClaimTypes.Name, "Mario");
claims.Remove(ClaimTypes.Role);

var principal = new ClaimsPrincipal(new ClaimsIdentity(claims));

var name = principal.GetClaimValue(ClaimTypes.Name);
var roles = principal.GetClaimValues(ClaimTypes.Role);
var hasName = principal.HasClaim(ClaimTypes.Name);
```

## Configuration and options

### `IConfigurationExtensions`

| Method | What it does | When to use it |
| --- | --- | --- |
| `GetSection<T>(sectionName)` | Reads a configuration section and materializes it as object `T`. | When you want a strongly typed object without registering it immediately in the container. |

### `ServiceCollectionExtensions`

| Method | What it does | When to use it |
| --- | --- | --- |
| `ConfigureAndGet<T>(configuration, sectionName)` | Binds a section and also registers the options in the container, returning the created instance. | When you want to configure and read the same options during startup. |
| `Replace<TService, TImplementation>(lifetime)` | Replaces a registered service with another implementation. | When you want to swap a service without rewriting the whole registration. |
| `AddDefaultProblemDetails()` | Registers `ProblemDetails` with defaults for common fields and a trace identifier. | When you want uniform RFC 7807 responses that are easy to correlate in clients and logs. |
| `AddDefaultExceptionHandler()` | Registers the library's default `IExceptionHandler` and ensures `ProblemDetails` is available too. | When you want unhandled exceptions to use the same payload shape as explicit errors. |
| `UseDefaults(ProblemDetailsContext context)` | Applies the library's default values to a `ProblemDetailsContext`. | When you customize `CustomizeProblemDetails` but still want the same base error contract. |
| `AddRequestLocalization(params string[] cultures)` | Registers localization using only the list of supported cultures. The first culture becomes the default. | When the default provider order is enough. |
| `AddRequestLocalization(IEnumerable<string> cultures, Action<IList<IRequestCultureProvider>>? providersConfiguration)` | Registers localization and lets you customize the culture-provider chain. The first culture becomes the default. | When you want a specific precedence, such as route values before cookies or the `Accept-Language` header. |

### Example

```csharp
var settings = builder.Services.ConfigureAndGet<MySettings>(builder.Configuration, "MySettings");

builder.Services.AddRequestLocalization("it-IT", "en-US");
builder.Services.AddDefaultProblemDetails();
builder.Services.AddDefaultExceptionHandler();
```

## Localization and pipeline

### `NavigationManagerExtensions`

| Method | What it does | When to use it |
| --- | --- | --- |
| `TryGetQueryString<T>(key, out value)` | Reads a value from the current query string and tries to convert it to `int`, `string`, `decimal`, or `bool`. | When you work in Blazor or components that need to read URL parameters. |

### `ApplicationBuilderExtensions`

| Method | What it does | When to use it |
| --- | --- | --- |
| `UseRequestRewind()` | Enables request-body buffering so the body can be read more than once. | When middleware or filters must inspect the body for validation, auditing, or request-signature checks without consuming it permanently. |

### `EnableRequestRewindMiddleware`

The internal middleware used by `UseRequestRewind()` calls `Request.EnableBuffering()` before passing control to the next middleware. It is useful for audit, validation, or request-signature scenarios.

### Example

```csharp
app.UseRequestRewind();

if (navManager.TryGetQueryString<int>("page", out var page))
{
    // use page
}
```

## Validation and authorization attributes

### `AllowedExtensionsAttribute`

Validates that an `IFormFile` uses one of the allowed file extensions. Use it when the file-name suffix is part of the upload contract and downstream processing or policy checks require known formats.

- Constructor: `AllowedExtensionsAttribute(params string[] extensions)`
- `FormatErrorMessage(string name)`: generates the error message with the allowed extensions

### `ContentTypeAttribute`

Validates the `Content-Type` of an `IFormFile`. Use it when the server accepts only MIME types that can be rendered, transcoded, stored, or otherwise processed safely.

- `ContentTypeAttribute(params string[] validContentTypes)`: uses an explicit MIME type list
- `ContentTypeAttribute(FileType fileType)`: uses a predefined group
- `FormatErrorMessage(string name)`: generates the error message with the allowed MIME types

### `FileType`

Supporting enum for the built-in `ContentTypeAttribute` MIME type groups:

- `Image`
- `Video`
- `Audio`

### `FileSizeAttribute`

Validates that an `IFormFile` does not exceed a maximum size. Use it to reject oversized uploads at the request boundary before later validation, storage, or media-processing work starts.

- Constructor: `FileSizeAttribute(int maxFileSizeInBytes)`
- `FormatErrorMessage(string name)`: generates the error message with the limit in bytes

### `RoleAuthorizeAttribute`

Automatically builds the `Roles` list of `AuthorizeAttribute` from one or more role values, keeping role-based authorization declarations readable without manually composing a comma-delimited string.

- Constructor: `RoleAuthorizeAttribute(params string[] roles)`

### Example

```csharp
using TinyHelpers.AspNetCore.DataAnnotations;

public sealed class UploadModel
{
    [AllowedExtensions("jpg", "png")]
    [ContentType(FileType.Image)]
    [FileSize(2_000_000)]
    public IFormFile File { get; set; } = default!;
}

[RoleAuthorize("Admin", "Manager")]
public IActionResult SecretArea()
{
    return Ok();
}
```

## Route and Minimal API helpers

### `RouteHandlerBuilderExtensions`

| Method | What it does | Availability |
| --- | --- | --- |
| `ProducesDefaultProblem(params int[] statusCodes)` | Adds `ProblemDetails` responses to the route metadata for expected failure status codes. | .NET 8+ |
| `WithResponseDescription(int statusCode, string description)` | Updates the description of an existing OpenAPI response when the generated text is too generic. | .NET 10+ |
| `WithLocationHeader(string description, int statusCode)` | Adds a required `Location` header to the specified creation response. | .NET 10+ |

These helpers keep Minimal API behavior and OpenAPI metadata close to the route mapping. This makes endpoint declarations easier to review and helps the generated document stay synchronized with runtime behavior.

### Example

```csharp
app.MapPost("/orders", () => Results.Created("/orders/1", new { Id = 1 }))
   .ProducesDefaultProblem(StatusCodes.Status400BadRequest, StatusCodes.Status500InternalServerError);
```

### .NET 10 example

```csharp
app.MapPost("/orders", () => Results.Created("/orders/1", new { Id = 1 }))
   .WithResponseDescription(StatusCodes.Status201Created, "Order created successfully")
   .WithLocationHeader();
```

## OpenAPI

The OpenAPI extensions are available when the consuming project uses the package on .NET 9 or .NET 10. They centralize reusable OpenAPI conventions so shared parameters, error responses, schema IDs, and schema transformations do not need to be repeated for every endpoint or document.

### `OpenApiExtensions`

| Method | What it does | Notes |
| --- | --- | --- |
| `AddOpenApiOperationParameters(setupAction)` | Registers reusable OpenAPI operation parameters inside `OpenApiOperationOptions.Parameters`. | Declare cross-cutting headers or query values once during service registration. |
| `AddAcceptLanguageHeader()` | Adds the `Accept-Language` header to documented operations. | Useful when the app uses request localization. |
| `AddDefaultProblemDetailsResponse()` | Adds a default error response based on `ProblemDetails`. | In .NET 9 it also adds the document transformer. |
| `AddOperationParameters()` | Adds the operation transformer that copies parameters registered with `AddOpenApiOperationParameters()` into generated operations. | Use it in OpenAPI configuration so the generated contract includes those shared inputs. |
| `RemoveServerList()` | Removes the server list from the OpenAPI document. | Useful when you want a more portable document across environments. |
| `WriteNumberAsString()` | Aligns the schema with numbers serialized as strings. | Useful when runtime JSON uses `JsonNumberHandling.WriteAsString`. |
| `DescribeAllParametersInCamelCase()` | Converts query parameter names to camel case in the document. | Keeps documentation consistent with JSON naming. |
| `AddTimeExamples()` | Adds readable examples for `TimeSpan` and `TimeOnly`. | Helps readers understand the expected format. |
| `EnableEnumSupport()` | Improves enum representation in the document. | .NET 9 only. |
| `UseFullTypeNameSchemaIds()` | Uses the full type name for schema IDs. | Prevents collisions between types with the same name. |
| `UseStrictNumericSchemas()` | Removes the `string` fallback from numeric schemas. | .NET 10 only. |

### `OpenApiSchemaHelper`

This utility creates ready-to-use schema fragments that can be reused in transformers or other OpenAPI customizations. It keeps default values, formats, and enum choices consistent across generated documents.

#### .NET 9

- `CreateStringSchema(string? defaultValue = null)`
- `CreateSchema<TValue>(string type, string? format = null)`
- `CreateSchema<TValue>(string type, string? format, TValue? defaultValue = null)`
- `CreateSchema(IEnumerable<string> values, string? defaultValue = null)`
- `CreateSchema<TEnum>(TEnum? defaultValue = null)`

#### .NET 10

- `CreateStringSchema(string? defaultValue = null)`
- `CreateSchema<TValue>(JsonSchemaType type, string? format = null)`
- `CreateSchema<TValue>(JsonSchemaType type, string? format, TValue? defaultValue = null)`
- `CreateSchema(IEnumerable<string> values, string? defaultValue = null)`
- `CreateSchema<TEnum>(TEnum? defaultValue = null)`

### What they do in practice

- `CreateStringSchema()` creates a reusable string schema, optionally with a default value.
- `CreateSchema(..., format)` creates a primitive schema with explicit type and format metadata.
- `CreateSchema(..., defaultValue)` also adds the default value that clients should display or assume.
- `CreateSchema(IEnumerable<string> values, ...)` turns externally defined choices into a string-based OpenAPI enum.
- `CreateSchema<TEnum>(...)` builds an enum schema directly from a CLR enum so every declared value is documented.

### Example

```csharp
builder.Services.AddOpenApi(options =>
{
    options.AddAcceptLanguageHeader();
    options.AddDefaultProblemDetailsResponse();
    options.AddOperationParameters();
    options.RemoveServerList();
    options.WriteNumberAsString();
    options.DescribeAllParametersInCamelCase();
    options.AddTimeExamples();
    options.UseFullTypeNameSchemaIds();
#if NET9_0
    options.EnableEnumSupport();
#endif
#if NET10_0_OR_GREATER
    options.UseStrictNumericSchemas();
#endif
});
```

### Example `AddOpenApiOperationParameters()`

Register shared parameters during service registration, then enable `AddOperationParameters()` in OpenAPI configuration. This keeps cross-cutting inputs centralized while still exposing them in the generated client contract.

```csharp
builder.Services.AddOpenApiOperationParameters(parameters =>
{
    parameters.Parameters.Add(new OpenApiParameter
    {
        Name = "X-Correlation-Id",
        In = ParameterLocation.Header,
        Required = false,
        Description = "Identifier used to correlate requests"
    });
});

builder.Services.AddOpenApi(options =>
{
    options.AddOperationParameters();
});
```

### Example `OpenApiSchemaHelper`

```csharp
var schema = OpenApiSchemaHelper.CreateStringSchema("it-IT");
```

## Exception handling

### `DefaultExceptionHandler`

`TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)` converts unhandled exceptions into a consistent `ProblemDetails` response.

Main behavior:

- uses the `StatusCode` from `BadHttpRequestException` when present
- sets `title`, `detail`, `instance`, and `traceId`
- adds `innerException` when an inner exception exists
- includes `stackTrace` only in the Development environment

### Example

```csharp
builder.Services.AddDefaultExceptionHandler();
app.UseExceptionHandler();
```

If a route throws an exception, the response becomes predictable and easy for clients to document and consume.

## Quick examples

### Minimal API with ProblemDetails and OpenAPI

```csharp
var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDefaultProblemDetails();
builder.Services.AddDefaultExceptionHandler();
builder.Services.AddRequestLocalization("it-IT", "en-US");

builder.Services.AddOpenApi(options =>
{
    options.AddAcceptLanguageHeader();
    options.AddDefaultProblemDetailsResponse();
    options.UseFullTypeNameSchemaIds();
});

var app = builder.Build();

app.UseExceptionHandler();
app.UseRequestRewind();

app.MapGet("/hello", () => Results.Ok(new { Message = "Hello" }))
   .ProducesDefaultProblem(StatusCodes.Status500InternalServerError);

app.Run();
```

### Reading a configuration section

```csharp
var appSettings = builder.Services.ConfigureAndGet<AppSettings>(builder.Configuration, "AppSettings");
```

## Contribute

The project is continuously evolving. Contributions, issues, and pull requests are welcome.

> [!WARNING]
> Work on the **develop** branch, not on **master**. Pull requests should target **develop**.
