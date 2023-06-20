namespace TinyHelpers.Http;

/// <summary>
/// Represents a handler for injecting headers in an HTTP request message.
/// </summary>
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
    /// Calls the function to get headers and then sends an HTTP request to the inner handler to send to the server as an asynchronous operation.
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
