# Tiny Helpers for Swashbuckle ASP.NET Core

[![Lint Code Base](https://github.com/marcominerva/TinyHelpers/actions/workflows/linter.yml/badge.svg)](https://github.com/marcominerva/TinyHelpers/actions/workflows/linter.yml)
[![CodeQL](https://github.com/marcominerva/TinyHelpers/actions/workflows/github-code-scanning/codeql/badge.svg)](https://github.com/marcominerva/TinyHelpers/actions/workflows/github-code-scanning/codeql)
[![NuGet](https://img.shields.io/nuget/v/TinyHelpers.AspNetCore.Swashbuckle.svg?style=flat-square)](https://www.nuget.org/packages/TinyHelpers.AspNetCore.Swashbuckle)
[![Nuget](https://img.shields.io/nuget/dt/TinyHelpers.AspNetCore.Swashbuckle)](https://www.nuget.org/packages/TinyHelpers.AspNetCore.Swashbuckle)
[![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg)](https://github.com/marcominerva/TinyHelpers/blob/master/LICENSE)

A collection of helper methods and classes for Swashbuckle ASP.NET Core that I use every day. I have packed them in a single library to avoid code duplication.

**Installation**

The library is available on [NuGet](https://www.nuget.org/packages/TinyHelpers.AspNetCore.Swashbuckle). Just search for *TinyHelpers.AspNetCore* in the **Package Manager GUI** or run the following command in the **.NET CLI**:

```shell
dotnet add package TinyHelpers.AspNetCore.Swashbuckle
```

**Usage**

The library provides some useful extension methods for Swashbuckle ASP.NET Core:

- `AddAcceptLanguageHeader`: an extension method for the Swagger implementation provided by [Swashbuckle.AspNetCore](https://github.com/domaindrivendev/Swashbuckle.AspNetCore). It adds the _Accept-Language_ header to **swagger.json** definition.

```csharp
builder.Services.AddSwaggerGen(options =>
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

**Contribute**

The project is constantly evolving. Contributions are welcome. Feel free to file issues and pull requests on the repo and we'll address them as we can. 

> **Warning**
Remember to work on the **develop** branch, don't use the **master** branch directly. Create Pull Requests targeting **develop**.
