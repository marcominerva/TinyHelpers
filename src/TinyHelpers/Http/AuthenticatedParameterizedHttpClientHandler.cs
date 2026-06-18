using System.Net;
using System.Net.Http.Headers;

namespace TinyHelpers.Http;

/// <summary>
/// Adds an authorization token to outgoing HTTP requests and can refresh the token once after an unauthorized response.
/// </summary>
/// <remarks>
/// Use this handler when token acquisition depends on the current <see cref="HttpRequestMessage" />, such as per-tenant
/// or per-resource tokens, and the client needs a lightweight retry path for expired credentials.
/// </remarks>
public class AuthenticatedParameterizedHttpClientHandler : DelegatingHandler
{
    /// <summary>
    /// The default authorization scheme used when the request does not already specify one.
    /// </summary>
    public const string BearerAuthorizationScheme = "Bearer";

    private readonly Func<HttpRequestMessage, Task<string>> getToken;
    private readonly Func<HttpRequestMessage, Task>? refreshToken;
    private readonly bool checkAuthorizationHeader;
    private readonly string authorizationScheme;

    /// <summary>
    /// Initializes a new handler that can add request-specific authorization tokens and optionally refresh them.
    /// </summary>
    /// <param name="getToken">Delegate used to get the access token for the current request.</param>
    /// <param name="refreshToken">Optional delegate used to refresh the access token after a <see cref="HttpStatusCode.Unauthorized" /> response.</param>
    /// <param name="checkAuthorizationHeader">Indicates whether a token should only be added when the request already has an authorization header.</param>
    /// <param name="authorizationScheme">The authorization scheme used for the HTTP request.</param>
    /// <exception cref="ArgumentNullException"><paramref name="getToken"/> is <see langword="null"/>.</exception>
    public AuthenticatedParameterizedHttpClientHandler(Func<HttpRequestMessage, Task<string>> getToken, Func<HttpRequestMessage, Task>? refreshToken = null, bool checkAuthorizationHeader = true, string authorizationScheme = BearerAuthorizationScheme)
    {
        this.getToken = getToken ?? throw new ArgumentNullException(nameof(getToken));
        this.refreshToken = refreshToken;
        this.checkAuthorizationHeader = checkAuthorizationHeader;
        this.authorizationScheme = authorizationScheme;
    }

    /// <summary>
    /// Initializes a new handler with a custom inner handler and request-specific token provider.
    /// </summary>
    /// <param name="getToken">Delegate used to get the access token for the current request.</param>
    /// <param name="innerHandler">The next handler in the HTTP pipeline.</param>
    /// <param name="authorizationScheme">The authorization scheme used for the HTTP request.</param>
    /// <exception cref="ArgumentNullException"><paramref name="getToken"/> is <see langword="null"/>.</exception>
    public AuthenticatedParameterizedHttpClientHandler(Func<HttpRequestMessage, Task<string>> getToken, HttpMessageHandler innerHandler, string authorizationScheme = BearerAuthorizationScheme)
        : this(getToken, null, true, innerHandler, authorizationScheme)
    {
    }

    /// <summary>
    /// Initializes a new handler with a custom inner handler, request-specific token provider, and token refresh callback.
    /// </summary>
    /// <param name="getToken">Delegate used to get the access token for the current request.</param>
    /// <param name="refreshToken">Delegate used to refresh the access token after a <see cref="HttpStatusCode.Unauthorized" /> response.</param>
    /// <param name="innerHandler">The next handler in the HTTP pipeline.</param>
    /// <param name="authorizationScheme">The authorization scheme used for the HTTP request.</param>
    /// <exception cref="ArgumentNullException"><paramref name="getToken"/> is <see langword="null"/>.</exception>
    public AuthenticatedParameterizedHttpClientHandler(Func<HttpRequestMessage, Task<string>> getToken, Func<HttpRequestMessage, Task>? refreshToken, HttpMessageHandler innerHandler, string authorizationScheme = BearerAuthorizationScheme)
        : this(getToken, refreshToken, true, innerHandler, authorizationScheme)
    {
    }

    /// <summary>
    /// Initializes a new handler with full control over token refresh, authorization-header behavior, and the inner handler.
    /// </summary>
    /// <param name="getToken">Delegate used to get the access token for the current request.</param>
    /// <param name="refreshToken">Delegate used to refresh the access token after a <see cref="HttpStatusCode.Unauthorized" /> response.</param>
    /// <param name="checkAuthorizationHeader">Indicates whether a token should only be added when the request already has an authorization header.</param>
    /// <param name="innerHandler">The next handler in the HTTP pipeline.</param>
    /// <param name="authorizationScheme">The authorization scheme used for the HTTP request.</param>
    /// <exception cref="ArgumentNullException"><paramref name="getToken"/> is <see langword="null"/>.</exception>
    public AuthenticatedParameterizedHttpClientHandler(Func<HttpRequestMessage, Task<string>> getToken, Func<HttpRequestMessage, Task>? refreshToken, bool checkAuthorizationHeader, HttpMessageHandler innerHandler, string authorizationScheme = BearerAuthorizationScheme)
        : base(innerHandler)
    {
        this.getToken = getToken ?? throw new ArgumentNullException(nameof(getToken));
        this.refreshToken = refreshToken;
        this.checkAuthorizationHeader = checkAuthorizationHeader;
        this.authorizationScheme = authorizationScheme;
    }

    /// <summary>
    /// Adds the configured authorization token before sending the request and retries once after refreshing the token when the response is unauthorized.
    /// </summary>
    /// <param name="request">The HTTP request message to send to the server.</param>
    /// <param name="cancellationToken">A cancellation token to cancel operation.</param>
    /// <returns>The <see cref="Task"/> object representing the asynchronous operation.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="request"/> is <see langword="null"/>.</exception>
    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        await CheckAddTokenAsync(request).ConfigureAwait(false);

        var response = await base.SendAsync(request, cancellationToken).ConfigureAwait(false);
        if (response.StatusCode == HttpStatusCode.Unauthorized && refreshToken is not null)
        {
            try
            {
                await refreshToken(request).ConfigureAwait(false);
                await CheckAddTokenAsync(request).ConfigureAwait(false);

                response = await base.SendAsync(request, cancellationToken).ConfigureAwait(false);
            }
            catch
            {
            }
        }

        return response;

        async Task CheckAddTokenAsync(HttpRequestMessage request)
        {
            // See if the request has an authorize header.
            var authorizationHeader = request.Headers.Authorization;
            if (authorizationHeader is not null || !checkAuthorizationHeader)
            {
                var token = await getToken(request).ConfigureAwait(false);
                request.Headers.Authorization = new AuthenticationHeaderValue(authorizationHeader?.Scheme ?? authorizationScheme, token);
            }
        }
    }
}
