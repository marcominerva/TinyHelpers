# Tiny Helpers for ASP.NET Core

[![Lint Code Base](https://github.com/marcominerva/TinyHelpers/actions/workflows/linter.yml/badge.svg)](https://github.com/marcominerva/TinyHelpers/actions/workflows/linter.yml)
[![CodeQL](https://github.com/marcominerva/TinyHelpers/actions/workflows/codeql.yml/badge.svg)](https://github.com/marcominerva/TinyHelpers/actions/workflows/codeql.yml)
[![NuGet](https://img.shields.io/nuget/v/TinyHelpers.AspNetCore.svg?style=flat-square)](https://www.nuget.org/packages/TinyHelpers.AspNetCore)
[![Nuget](https://img.shields.io/nuget/dt/TinyHelpers.AspNetCore)](https://www.nuget.org/packages/TinyHelpers.AspNetCore)
[![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg)](https://github.com/marcominerva/TinyHelpers/blob/master/LICENSE)

A collection of helper methods and classes for ASP.NET Core that I use every day. I have packed them in a single library to avoid code duplication.

> [!IMPORTANT]
> **Update from Version 3.x to 4.x**
> Swashbuckle (Swagger) support has been moved out from TinyHelpers.AspNetCore. If you're using extension methods like `AddAcceptLanguageHeader` and `AddDefaultResponse` with `AddSwaggerGen`, now you need to install the [TinyHelpers.AspNetCore.Swashbuckle](https://github.com/marcominerva/TinyHelpers/tree/master/src/TinyHelpers.AspNetCore.Swashbuckle) package.

**Installation**

The library is available on [NuGet](https://www.nuget.org/packages/TinyHelpers.AspNetCore). Just search for *TinyHelpers.AspNetCore* in the **Package Manager GUI** or run the following command in the **.NET CLI**:

```shell
dotnet add package TinyHelpers.AspNetCore
```

**Usage**

The library provides some useful extension methods for ASP.NET Core applications:

- `AddDefaultProblemDetails`: calls the default [ProblemDetails](https://learn.microsoft.com/dotnet/api/microsoft.extensions.dependencyinjection.problemdetailsservicecollectionextensions) extension method defining the [CustomizeProblemDetails](https://learn.microsoft.com/dotnet/api/microsoft.aspnetcore.http.problemdetailsoptions.customizeproblemdetails) property to ensure that all the typical properties (_Type_, _Title_, _Instance_, _TraceId_) are correctly set.

- `AddDefaultExceptionHandler`: adds a default implementation for the [IExceptionHandler](https://learn.microsoft.com/dotnet/api/microsoft.aspnetcore.diagnostics.iexceptionhandler) interface that returns the following response on exceptions:

```csharp
app.MapPost("/api/exception", () =>
{
    throw new Exception("This is an exception", innerException: new HttpRequestException("This is an inner exception"));
});
```
```json
{
  "type": "https://tools.ietf.org/html/rfc9110#section-15.6.1",
  "title": "System.Exception",
  "status": 500,
  "detail": "This is an exception",
  "instance": "/api/exception",
  "traceId": "00-1bffa22c7288e5784bf763d5fd05bc87-87da01a1b76d1692-00",
  "innerException": "System.Net.Http.HttpRequestException",
  "innerExceptionMessage": "This is an inner exception",
  "stackTrace": "..."
}
```

> **Note**
The _StackTrace_ property is included in the response only if the application is running in the _Development_ environment.

- `AddRequestLocalization(cultures)`: an overload of the standard [AddRequestLocalization](https://learn.microsoft.com/dotnet/api/microsoft.extensions.dependencyinjection.requestlocalizationservicecollectionextensions) extension method that just requires the list of supported cultures and automatically registers the corresponding [RequestLocalizationOptions](https://learn.microsoft.com/dotnet/api/microsoft.aspnetcore.builder.requestlocalizationoptions) (Note: The first culture in the list is used as the default culture).

- `ConfigureAndGet<TOptions>(  )`: it allows to register a configuration instance on type _TOptions_ and returns the instance itself. It is useful when you need to configure the options and gets the reference to that instance in the same method, typically at application startup.

- `AddAcceptLanguageHeader()`: adds the _Accept-Language_ header to OpenAPI definition:

```csharp
builder.Services.AddOpenApi(options =>
{
    options.AddAcceptLanguageHeader();
});
```

- `AddDefaultProblemDetailsResponse()`: adds a default (error) response to all endpoints in the OpenAPI definition:

```csharp
builder.Services.AddOpenApi(options =>
{
    options.AddDefaultProblemDetailsResponse();
});
```

- `UseFullTypeNameSchemaIds()`: configures OpenAPI to use the full type name (including namespace) for schema reference IDs, helping to avoid naming collisions when multiple types have the same name:

```csharp
builder.Services.AddOpenApi(options =>
{
    options.UseFullTypeNameSchemaIds();
});
```

This is particularly useful when you have multiple types with the same name in different namespaces. For example, if you have both `MyApp.Models.User` and `MyApp.DTOs.User`, the default behavior would create a single schema named "User", potentially causing conflicts. With `UseFullTypeNameSchemaIds()`, the schemas will be named "MyApp.Models.User" and "MyApp.DTOs.User" respectively.

**Contribute**

The project is constantly evolving. Contributions are welcome. Feel free to file issues and pull requests on the repo and we'll address them as we can. 

> **Warning**
Remember to work on the **develop** branch, don't use the **master** branch directly. Create Pull Requests targeting **develop**.