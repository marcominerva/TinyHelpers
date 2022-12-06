# Tiny Helpers for Dapper

[![Lint Code Base](https://github.com/marcominerva/TinyHelpers/actions/workflows/linter.yml/badge.svg)](https://github.com/marcominerva/TinyHelpers/actions/workflows/linter.yml)
[![CodeQL](https://github.com/marcominerva/TinyHelpers/actions/workflows/codeql.yml/badge.svg)](https://github.com/marcominerva/TinyHelpers/actions/workflows/codeql.yml)
[![NuGet](https://img.shields.io/nuget/v/TinyHelpers.Dapper.svg?style=flat-square)](https://www.nuget.org/packages/TinyHelpers.Dapper)
[![Nuget](https://img.shields.io/nuget/dt/TinyHelpers.Dapper)](https://www.nuget.org/packages/TinyHelpers.Dapper)
[![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg)](https://github.com/marcominerva/TinyHelpers/blob/master/LICENSE)

A collection of helper methods and classes for Dapper that I use every day. I have packed them in a single library to avoid code duplication.

**Installation**

The library is available on [NuGet](https://www.nuget.org/packages/TinyHelpers.Dapper). Just search for *TinyHelpers.Dapper* in the **Package Manager GUI** or run the following command in the **.NET CLI**:

    dotnet add package TinyHelpers.Dapper

**Usage**

The library provides some Type Handlers to handle data types that are not natively supported by Dapper. They can be explicitly used calling the corresponding `Configure` methods:

    // using TinyHelpers.Dapper.TypeHandlers;

    // Automatically serializes and deserializes Person class in JSON string.
    JsonTypeHandler.Configure<Person>();

    // Handles all string[] properties.
    StringArrayTypeHandler.Configure();

    // Handles all IEnumerable<string> properties.
    StringEnumerableTypeHandler.Configure();

    // Handles DateOnly and TimeOnly propertites.
    DateOnlyTypeHandler.Configure();
    TimeOnlyTypeHandler.Configure();

**Contribute**

The project is constantly evolving. Contributions are welcome. Feel free to file issues and pull requests on the repo and we'll address them as we can. 
