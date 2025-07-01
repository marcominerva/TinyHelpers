## General

- Make only high confidence suggestions when reviewing code changes.
- Always use the latest version C#, currently C# 13 features.
- Write code that is clean, maintainable, and easy to understand.
- Only add comments rarely to explain why a non-intuitive solution was used. The code should be self-explanatory otherwise.
- Don't add the UTF-8 BOM to files unless they have non-ASCII characters.
- Never change global.json unless explicitly asked to.
- Never change package.json or package-lock.json files unless explicitly asked to.
- Never change NuGet.config files unless explicitly asked to.

## Code Style

### Formatting

- Apply code-formatting style defined in `.editorconfig`.
- Use primary constructors where applicable.
- Prefer file-scoped namespace declarations and single-line using directives.
- Insert a newline before the opening curly brace of any code block (e.g., after `if`, `for`, `while`, `foreach`, `using`, `try`, etc.).
- Ensure that the final return statement of a method is on its own line.
- Use pattern matching and switch expressions wherever possible.
- Prefer using collection expressions when possible
- Use `is` pattern matching instead of `as` and null checks
- Use `nameof` instead of string literals when referring to member names.
- Prefer `?.` if applicable (e.g. `scope?.Dispose()`).
- Use `ObjectDisposedException.ThrowIf` where applicable.
- Use `ArgumentNullException.ThrowIfNull` to validate input paramters.
- If you add new code files, ensure they are listed in the csproj file (if other files in that folder are listed there) so they build.

### Nullable Reference Types

- Declare variables non-nullable, and check for `null` at entry points.
- Always use `is null` or `is not null` instead of `== null` or `!= null`.
- Trust the C# null annotations and don't add null checks when the type system says a value cannot be null.

## Architecture and Design Patterns

### Asynchronous Programming

- Provide both synchronous and asynchronous versions of methods where appropriate.
- Use the `Async` suffix for asynchronous methods.
- Return `Task` or `ValueTask` from asynchronous methods.
- Use `CancellationToken` parameters to support cancellation.
- Avoid async void methods except for event handlers.
- Call `ConfigureAwait(false)` on awaited calls to avoid deadlocks.

### Error Handling

- Use appropriate exception types. 
- Include helpful error messages.
- Avoid catching exceptions without rethrowing them.

### Performance Considerations

- Be mindful of performance implications, especially for database operations.
- Avoid unnecessary allocations.
- Consider using more efficient code that is expected to be on the hot path, even if it is less readable.

### Implementation Guidelines

- Write code that is secure by default. Avoid exposing potentially private or sensitive data.
- Make code NativeAOT compatible when possible. This means avoiding dynamic code generation, reflection, and other features that are not compatible. with NativeAOT. If not possible, mark the code with an appropriate annotation or throw an exception.

## Documentation

- Include XML documentation for all public APIs. Mention the purpose, intent, and 'the why' of the code, so developers unfamiliar with the project can better understand it. If comments already exist, update them to meet the before mentioned criteria if needed. Use the full syntax of XML Doc Comments to make them as awesome as possible including references to types. Don't add any documentation that is obvious for even novice developers by reading the code.
- Add proper `<remarks>` tags with links to relevant documentation where helpful.
- For keywords like `null`, `true` or `false` use `<see langword="*" />` tags.
- Include code examples in documentation where appropriate.
- Overriding members should inherit the XML documentation from the base type via `/// <inheritdoc />`.

## Markdown
- Use Markdown for documentation files (e.g., README.md).
- Use triple backticks for code blocks, JSON snippets and bash commands, specifying the language (e.g., ```csharp, ```json and ```bash).

## Testing

- When adding new unit tests, strongly prefer to add them to existing test code files rather than creating new code files.
- We use xUnit SDK v3 for tests.
- Do not emit "Act", "Arrange" or "Assert" comments.
- Use NSubstitute for mocking in tests.
- Copy existing style in nearby files for test method names and capitalization.
- When running tests, if possible use filters and check test run counts, or look at test logs, to ensure they actually ran.
- Do not finish work with any tests commented out or disabled that were not previously commented out or disabled.