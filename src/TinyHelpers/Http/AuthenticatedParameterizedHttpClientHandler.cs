using System.Net;
using System.Net.Http.Headers;

namespace TinyHelpers.Http;

/// <summary>
/// Represents a handler to authenticate HTTP requests using Bearer token.
/// </summary>
public class AuthenticatedParameterizedHttpClientHandler : DelegatingHandler
{
    /// <summary>
    /// Scheme for Bearer authorization.
    /// </summary>
    public const string BearerAuthorizationScheme = "Bearer";

    private readonly Func<HttpRequestMessage, Task<string>> getToken;
    private readonly Func<HttpRequestMessage, Task>? refreshToken;
    private readonly bool checkAuthorizationHeader;
    private readonly string authorizationScheme;

    /// <summary>
    /// Initializes a new instance of the <see cref="AuthenticatedParameterizedHttpClientHandler"/> class.
    /// </summary>
    /// <param name="getToken">Delegate function to get access token for authentication.</param>
    /// <param name="refreshToken">Optional delegate function to refresh access token.</param>
    /// <param name="checkAuthorizationHeader">Indicates if there should be an Authorization header.</param>
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
    /// Constructor for AuthenticatedParameterizedHttpClientHandler.
    /// </summary>
    /// <param name="getToken">Delegate function to get access token for authentication.</param>
    /// <param name="innerHandler">Inner handler to send request to.</param>
    /// <param name="authorizationScheme">The authorization scheme used for the HTTP request.</param>
    /// <exception cref="ArgumentNullException"><paramref name="getToken"/> is <see langword="null"/>.</exception>
    public AuthenticatedParameterizedHttpClientHandler(Func<HttpRequestMessage, Task<string>> getToken, HttpMessageHandler innerHandler, string authorizationScheme = BearerAuthorizationScheme)
        : this(getToken, null, true, innerHandler, authorizationScheme)
    {
    }

    /// <summary>
    /// Constructor for AuthenticatedParameterizedHttpClientHandler.
    /// </summary>
    /// <param name="getToken">Delegate function to get access token for authentication.</param>
    /// <param name="refreshToken">Delegate function to refresh access token.</param>
    /// <param name="innerHandler">Inner handler to send request to.</param>
    /// <param name="authorizationScheme">The authorization scheme used for the HTTP request.</param>
    /// <exception cref="ArgumentNullException"><paramref name="getToken"/> is <see langword="null"/>.</exception>
    public AuthenticatedParameterizedHttpClientHandler(Func<HttpRequestMessage, Task<string>> getToken, Func<HttpRequestMessage, Task>? refreshToken, HttpMessageHandler innerHandler, string authorizationScheme = BearerAuthorizationScheme)
        : this(getToken, refreshToken, true, innerHandler, authorizationScheme)
    {
    }

    /// <summary>
    /// Constructor for AuthenticatedParameterizedHttpClientHandler.
    /// </summary>
    /// <param name="getToken">Delegate function to get access token for authentication.</param>
    /// <param name="refreshToken">Delegate function to refresh access token.</param>
    /// <param name="checkAuthorizationHeader">Indicates if there should be an Authorization header.</param>
    /// <param name="innerHandler">Inner handler for HTTP message request to be sent to.</param>
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
    /// Calls the function to automatically add the Bearer token and then sends an HTTP request to the inner handler to send to the server as an asynchronous operation. If the response is 401 (Unauthorized), it will try to refresh the token using the handler specified in the <see cref="AuthenticatedParameterizedHttpClientHandler"/> constructor and try again.
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
