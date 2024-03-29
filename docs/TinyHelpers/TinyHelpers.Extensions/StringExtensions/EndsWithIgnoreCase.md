# StringExtensions.EndsWithIgnoreCase method

Determines whether the end of this string instance matches the specified string, performing a case-insentive comparison.

```csharp
public static bool EndsWithIgnoreCase(this string? input, string value)
```

| parameter | description |
| --- | --- |
| input | The string to check. |
| value | The string to compare. |

## Return Value

`true` if this instance ends with *value*, regardless the casing; otherwise, `false`. If *input* is `null`, the method returns `false`.

## See Also

* class [StringExtensions](../StringExtensions.md)
* namespace [TinyHelpers.Extensions](../../TinyHelpers.md)

<!-- DO NOT EDIT: generated by xmldocmd for TinyHelpers.dll -->
