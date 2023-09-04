# CollectionExtensions.GetLongCount&lt;TSource&gt; method

Gets the number of elements in the *source* collection.

```csharp
public static long GetLongCount<TSource>(this IEnumerable<TSource>? source, 
    Func<TSource, bool>? predicate = null)
```

| parameter | description |
| --- | --- |
| TSource | The type of the elements. |
| source | The IEnumerable to check. |
| predicate | The delegate function that defines the conditions of the elements to consider for the count. |

## Return Value

A Int64 representing the number of elements that meet the criteria defined by the *predicate* function, if not `null`; the number of elements in *source*, otherwise.

## See Also

* class [CollectionExtensions](../CollectionExtensions.md)
* namespace [TinyHelpers.Extensions](../../TinyHelpers.md)

<!-- DO NOT EDIT: generated by xmldocmd for TinyHelpers.dll -->