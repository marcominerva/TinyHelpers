# StringExtensions.ContainsIgnoreCase method

Determines whether this string instance contains the specified string, performing a case-insentive comparison.

```csharp
public static bool ContainsIgnoreCase(this string? input, string value)
```

| parameter | description |
| --- | --- |
| input | The string to check. |
| value | The string to compare. |

## Return Value

`true` if this instance contains *value*, regardless the casing; otherwise, `false`. If *input* is `null`, the method returns `false`.

## See Also

* class [StringExtensions](../StringExtensions.md)
* namespace [TinyHelpers.Extensions](../../TinyHelpers.md)

<!-- DO NOT EDIT: generated by xmldocmd for TinyHelpers.dll -->
