namespace TinyHelpers.Http;

/// <summary>
/// Adds request-specific headers to outgoing HTTP requests before they continue through the client pipeline.
/// </summary>
/// <remarks>
/// Use this handler to centralize cross-cutting headers such as correlation identifiers, tenant identifiers, or custom
/// API metadata without repeating header setup at every call site.
/// </remarks>
public class HeaderInjectorHttpClientHandler : DelegatingHandler
{
    private readonly Func<HttpRequestMessage, Task<Dictionary<string, string>>> getHeaders;

    /// <summary>
    /// Creates a new instance of the <see cref="HeaderInjectorHttpClientHandler"/> class.
    /// </summary>
    /// <param name="getHeaders">A function to get the headers to inject in the request message.</param>
    /// <exception cref="ArgumentNullException"><paramref name="getHeaders"/> is null.</exception>
    public HeaderInjectorHttpClientHandler(Func<HttpRequestMessage, Task<Dictionary<string, string>>> getHeaders)
    {
        this.getHeaders = getHeaders ?? throw new ArgumentNullException(nameof(getHeaders));
    }

    /// <summary>
    /// Creates a new instance of the <see cref="HeaderInjectorHttpClientHandler"/> class.
    /// </summary>
    /// <param name="getHeaders">A function to get the headers to inject in the request message.</param>
    /// <param name="innerHandler">Inner handler to send request to.</param>
    /// <exception cref="ArgumentNullException"><paramref name="getHeaders"/> is null.</exception>
    public HeaderInjectorHttpClientHandler(Func<HttpRequestMessage, Task<Dictionary<string, string>>> getHeaders, HttpMessageHandler innerHandler)
        : base(innerHandler)
    {
        this.getHeaders = getHeaders ?? throw new ArgumentNullException(nameof(getHeaders));
    }

    /// <summary>
    /// Resolves headers for the current request, adds them without strict validation, and sends the request to the next handler.
    /// </summary>
    /// <param name="request">The HTTP request message to send to the server.</param>
    /// <param name="cancellationToken">A cancellation token to cancel operation.</param>
    /// <returns>The <see cref="Task"/> object representing the asynchronous operation.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="request"/> is <see langword="null"/>.</exception>
    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        var headers = await getHeaders(request).ConfigureAwait(false);
        if (headers is not null)
        {
            foreach (var header in headers)
            {
                request.Headers.TryAddWithoutValidation(header.Key, header.Value);
            }
        }

        return await base.SendAsync(request, cancellationToken).ConfigureAwait(false);
    }
}
