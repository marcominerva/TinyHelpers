# TinyHelpers assembly

## TinyHelpers namespace

| public type | description |
| --- | --- |
| struct [WithIndex&lt;TValue&gt;](./TinyHelpers/WithIndex-1.md) | Represents a generic object with its relative index within a collection. |

## TinyHelpers.Extensions namespace

| public type | description |
| --- | --- |
| static class [CollectionExtensions](./TinyHelpers.Extensions/CollectionExtensions.md) | Contains extension methods for collections. |
| static class [DateTimeExtensions](./TinyHelpers.Extensions/DateTimeExtensions.md) | Contains extension methods for the DateTime type. |
| static class [EnumExtensions](./TinyHelpers.Extensions/EnumExtensions.md) | Contains extension methods for the Enum type. |
| static class [StopwatchExtensions](./TinyHelpers.Extensions/StopwatchExtensions.md) | Contains extension methods for managing the Stopwatch object. |
| static class [StringExtensions](./TinyHelpers.Extensions/StringExtensions.md) | Contains extensions methods for the String type. |
| static class [TaskExtensions](./TinyHelpers.Extensions/TaskExtensions.md) | Contains extension methods for the Task type. |

## TinyHelpers.Http namespace

| public type | description |
| --- | --- |
| class [AuthenticatedParameterizedHttpClientHandler](./TinyHelpers.Http/AuthenticatedParameterizedHttpClientHandler.md) | Represents a handler to authenticate HTTP requests using Bearer token. |
| class [HeaderInjectorHttpClientHandler](./TinyHelpers.Http/HeaderInjectorHttpClientHandler.md) | Represents a handler for injecting headers in an HTTP request message. |
| class [QueryStringInjectorHttpClientHandler](./TinyHelpers.Http/QueryStringInjectorHttpClientHandler.md) | Represents a handler for adding query string parameters to an HTTP request message. |

## TinyHelpers.Json.Serialization namespace

| public type | description |
| --- | --- |
| class [ShortDateConverter](./TinyHelpers.Json.Serialization/ShortDateConverter.md) | Converts a DateTime value to or from JSON, keeping only the date part. |
| class [StringEnumMemberConverter](./TinyHelpers.Json.Serialization/StringEnumMemberConverter.md) | Converts an Enum value to or from JSON, keeping only the date part. |
| class [StringTrimmingConverter](./TinyHelpers.Json.Serialization/StringTrimmingConverter.md) | A converter to trim the whitespace from JSON strings during serialization and deserialization. |
| class [TimeSpanTicksConverter](./TinyHelpers.Json.Serialization/TimeSpanTicksConverter.md) | A converter for serializing and deserializing TimeSpan as ticks. |
| class [UtcDateTimeConverter](./TinyHelpers.Json.Serialization/UtcDateTimeConverter.md) | A converter for serializing and deserializing DateTime values converting them to UTC, if needed. |

## TinyHelpers.Threading namespace

| public type | description |
| --- | --- |
| class [AsyncLock](./TinyHelpers.Threading/AsyncLock.md) | Provides a lock that can be used asynchronously. |

<!-- DO NOT EDIT: generated by xmldocmd for TinyHelpers.dll -->
