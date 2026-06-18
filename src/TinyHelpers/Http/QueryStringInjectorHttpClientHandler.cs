using System.Web;

namespace TinyHelpers.Http;

/// <summary>
/// Adds request-specific query string parameters to outgoing HTTP requests before they continue through the client pipeline.
/// </summary>
/// <remarks>
/// Use this handler when query values depend on the current request context and URL composition should be centralized,
/// such as pagination, tenant selection, or feature flags.
/// </remarks>
public class QueryStringInjectorHttpClientHandler : DelegatingHandler
{
    private readonly Func<HttpRequestMessage, Task<Dictionary<string, string>>> getQueryString;

    /// <summary>
    /// Creates a new instance of the <see cref="QueryStringInjectorHttpClientHandler"/> class.
    /// </summary>
    /// <param name="getQueryString">A function to get the query string parameters to add to the request message.</param>
    /// <exception cref="ArgumentNullException"><paramref name="getQueryString"/> is null.</exception>
    public QueryStringInjectorHttpClientHandler(Func<HttpRequestMessage, Task<Dictionary<string, string>>> getQueryString)
    {
        this.getQueryString = getQueryString ?? throw new ArgumentNullException(nameof(getQueryString));
    }

    /// <summary>
    /// Creates a new instance of the <see cref="QueryStringInjectorHttpClientHandler"/> class.
    /// </summary>
    /// <param name="getQueryString">A function to get the query string parameters to add to the request message.</param>
    /// <param name="innerHandler">Inner handler to send request to.</param>
    /// <exception cref="ArgumentNullException"><paramref name="getQueryString"/> is null.</exception>
    public QueryStringInjectorHttpClientHandler(Func<HttpRequestMessage, Task<Dictionary<string, string>>> getQueryString, HttpMessageHandler innerHandler)
        : base(innerHandler)
    {
        this.getQueryString = getQueryString ?? throw new ArgumentNullException(nameof(getQueryString));
    }

    /// <summary>
    /// Merges query string parameters for the current request into the URI and sends the request to the next handler.
    /// </summary>
    /// <param name="request">The HTTP request message to send to the server.</param>
    /// <param name="cancellationToken">A cancellation token to cancel operation.</param>
    /// <returns>The <see cref="Task"/> object representing the asynchronous operation.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="request"/> is <see langword="null"/>.</exception>
    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        var queryStringParameters = HttpUtility.ParseQueryString(request.RequestUri!.Query);

        var queryString = await getQueryString(request).ConfigureAwait(false);
        if (queryString is not null)
        {
            foreach (var parameter in queryString)
            {
                queryStringParameters.Add(parameter.Key, parameter.Value);
            }
        }

        var uriBuilder = new UriBuilder(request.RequestUri)
        {
            Query = queryStringParameters.ToString()
        };

        request.RequestUri = new Uri(uriBuilder.ToString());
        return await base.SendAsync(request, cancellationToken).ConfigureAwait(false);
    }
}
